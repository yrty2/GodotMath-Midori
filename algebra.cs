using Godot;
using System;
using mathematics;
using System.Numerics;
//群・環・体
public interface IMagma<M> where M:IMagma<M>{
	static abstract M operator *(M a,M b);
}
public interface IQuasigroup<Q>:IMagma<Q> where Q:IQuasigroup<Q>{
	Q inverse();
}
public interface ILoop<Q>:IQuasigroup<Q> where Q:ILoop<Q>{
	Q identity();
}
public interface ISemigroup<S>:IMagma<S> where S:ISemigroup<S>{
	static void Law(){
		GD.Print("Associative:(a*b)*c=a*(b*c)");
	}
}
public interface IMonoid<S>:ISemigroup<S> where S:IMonoid<S>{
	S identity();
}
public interface IGroup<G>:IMonoid<G> where G:IGroup<G>{
	G inverse();
}
public interface IAbelianGroup<A> where A:IAbelianGroup<A>{
	static void Law(){
		GD.Print("Associative:(a*b)*c=a*(b*c)");
		GD.Print("Commutative:a*b=b*a");
	}
    static abstract A operator +(A a,A b);
    static abstract A operator -(A a,A b);
    static abstract A operator -(A a);
    static abstract A zero {get;} //加法的な単位元を意味する
}
public interface IRing<T>:IMultiplyOperators<T,T,T> where T:IRing<T>{
	static void Law(){
		GD.Print("Associative+:(a+b)+c=a+(b+c)");
		GD.Print("Commutative+:a+b=b+a");
		GD.Print("Associative*:(a*b)*c=a*(b*c)");
		GD.Print("Distributive1:a*(b+c)=a*b+a*c");
		GD.Print("Distributive2:(a+b)*c=a*c+b*c");
	}
	static abstract T operator +(T a,T b);
    static abstract T operator -(T a,T b);
    static abstract T operator *(T a,T b);
	static abstract T zero {get;}
    static abstract T one {get;}
}
public interface ICommutativeRing<T>:IRing<T> where T:ICommutativeRing<T>{
	static new void Law(){
		GD.Print("Associative+:(a+b)+c=a+(b+c)");
		GD.Print("Commutative+:a+b=b+a");
		GD.Print("Associative*:(a*b)*c=a*(b*c)");
		GD.Print("Commutative*:a*b=b*a");
		GD.Print("Distributive1:a*(b+c)=a*b+a*c");
		GD.Print("Distributive2:(a+b)*c=a*c+b*c");
	}
}
public interface IField<T>:IRing<T>,IDivisionOperators<T,T,T> where T:IField<T>{
	static abstract T operator /(T a,T b);//乗法逆元
}
public interface IAlgebra<A,K>:IModule<A,K> where A:IAlgebra<A,K> where K:IMultiplyOperators<K,K,K>,IDivisionOperators<K,K,K>{
    static abstract A operator *(A a,A b);
}
public interface IModule<M,R>:IAbelianGroup<M> where M:IModule<M,R> where R:IMultiplyOperators<R,R,R>{
    static abstract M operator *(R a,M b);
	static abstract M operator *(M a,R b);
	static abstract R one {get;}//乗法的単位元
}
public interface IVectorSpace<V,R>:IModule<V,R> where V:IVectorSpace<V,R> where R:IMultiplyOperators<R,R,R>,IDivisionOperators<R,R,R>{}
public interface INormedVectorSpace<V,R>:IVectorSpace<V,R> where V:INormedVectorSpace<V,R> where R:IMultiplyOperators<R,R,R>,IDivisionOperators<R,R,R>{
	static R length;
}
public interface IMetricVectorSpace<V,R>:INormedVectorSpace<V,R> where V:IMetricVectorSpace<V,R> where R:IMultiplyOperators<R,R,R>,IDivisionOperators<R,R,R>{
	static abstract double operator |(V a,V b);//内積
}
public interface ICategory<O,M>{}
//束
public interface ILattice<T>{
    static abstract T meet(T a,T b);
    static abstract T join(T a,T b);
}
public interface IBooleanAlgebra<T>:ILattice<T>{
    static abstract T Not(T a);
    static abstract T Zero { get; }
    static abstract T One { get; }
}
//有理数
public readonly struct RInt:IField<RInt>{
	public readonly int a,b;
	public RInt(int A,int B){
		if(B==0){
    		throw new ArgumentException("RIntはゼロ除算を許しません");
		}
		a=A;
		b=B;
	}
	//乗法及び加法の単位元
	public static RInt one=>new(1,1);
	public static RInt zero=>new(0,1);
	//二項演算
	public static RInt operator +(RInt x,RInt y)=>new(x.a*y.b+y.a*x.b,x.b*y.b);
	public static RInt operator -(RInt x,RInt y)=>new(x.a*y.b-y.a*x.b,x.b*y.b);
	public static RInt operator *(RInt x,RInt y)=>new(x.a*y.a,x.b*y.b);
	public static RInt operator /(RInt x,RInt y)=>new(x.a*y.b,x.b*y.a);
	//等号、順序
	public static bool operator ==(RInt x,RInt y)=>x.a/x.b==y.a/y.b;
	public static bool operator !=(RInt x,RInt y)=>x.a/x.b!=y.a/y.b;
	public static bool operator <(RInt x,RInt y)=>x.a/x.b<y.a/y.b;
	public static bool operator >(RInt x,RInt y)=>x.a/x.b>y.a/y.b;
	public static bool operator <=(RInt x,RInt y)=>x.a/x.b<=y.a/y.b;
	public static bool operator >=(RInt x,RInt y)=>x.a/x.b>=y.a/y.b;
	public override int GetHashCode()=>HashCode.Combine(a,b);
	public override bool Equals(object obj){
        if(obj is RInt p){
            return a==p;
		}
        return false;
    }
	//utility
	public static implicit operator RInt(int a)=>new(a,1);
	public static implicit operator RInt(uint a)=>new((int)a,1);
	public static implicit operator double(RInt a)=>a.a/a.b;
}
//代数
public readonly struct Complex:IAlgebra<Complex,double>{
	public readonly vec2 z;
	public static Complex zero=>0;
	public static double one=>1;
	public Complex(double real, double imag){
		z=new vec2(real,imag);
	}
	public Complex(vec2 Z){
		z=Z;
	}
	public static implicit operator Complex(double real)=>
	new(real,0);

	public static implicit operator Complex(vec2 p)=>new(p);
	
	public static implicit operator Complex((double,double) t) =>
	new(t.Item1,t.Item2);
	
	public double real=>z.x;
	public double imag=>z.y;
	
	public static Complex operator +(Complex a, Complex b)=>new(a.z+b.z);
	public static Complex operator -(Complex a, Complex b)=>new(a.z-b.z);
	public static Complex operator -(Complex a)=>new(-a.z);
	public static Complex operator *(Complex a, Complex b)=>new(a.real*b.real-a.imag*b.imag,a.real*b.imag+a.imag*b.real);
	public static Complex operator *(Complex a, double b)=>new(b*a.real,b*a.imag);
	public static Complex operator *(double a, Complex b)=>new(a*b.real,a*b.imag);
	public static Complex operator /(Complex a, Complex b)=>a*~b/Mdi.dot(b.z,b.z);
	public static Complex operator /(Complex a, double b)=>new(a.real/b,a.imag/b);
	public static Complex operator ~(Complex a)=>Mdi.conj(a);
	public static Complex operator ^(Complex a,Complex b)=>Mdi.pow(a,b);
	public static Complex operator ^(Complex a,double b)=>Mdi.pow(a,b);
	public static Complex operator ^(double a,Complex b)=>Mdi.pow(a,b);
	public static bool operator ==(Complex a,Complex b){
		return a.z==b.z;
	}
	public static bool operator !=(Complex a,Complex b){
		return a.z!=b.z;
	}
	public override bool Equals(object obj){
    	return z.Equals(obj);
	}
	public override int GetHashCode(){
    	return z.GetHashCode();
	}
	public override string ToString() => $"{z.x}+{z.y}i";
}
public readonly struct Quaternion:IAlgebra<Quaternion,double>{
	public readonly vec4 q;
	public static Quaternion zero=>0;
	public static double one=>1;
	public Quaternion(double real,double i,double j,double k){
		q=new vec4(real,i,j,k);
	}
	public Quaternion(double real,vec3 v){
		q=new vec4(real,v);
	}
    public Quaternion(vec3 v){
		q=new vec4(0,v);
	}
	public Quaternion(vec4 Q){
		q=Q;
	}
	public Quaternion(Complex v){
		q=new vec4(v.real,v.imag,0,0);//所説あり
	}
    public static implicit operator Quaternion(double real)=>new(real,0,0,0);
    public static implicit operator Godot.Quaternion(Quaternion a)=>new(a.imag,(float)a.real);
    public static implicit operator Quaternion(Godot.Quaternion a)=>new(a.W,a.X,a.Y,a.Z);
	public static implicit operator Quaternion((double,double,double,double) t)=>new(t.Item1,t.Item2,t.Item3,t.Item4);
	
	public double real=>q.x;
	public vec3 imag=>q.yzw;
	public double i=>q.y;
	public double j=>q.z;
	public double k=>q.w;

	public static Quaternion operator +(Quaternion a,Quaternion b)=>
	new Quaternion(a.q+b.q);
	public static Quaternion operator -(Quaternion a,Quaternion b)=>
	new Quaternion(a.q-b.q);
	public static Quaternion operator -(Quaternion a)=>
	new Quaternion(-a.q);
	public static Quaternion operator *(Quaternion a,Quaternion b)=>
	new Quaternion(a.real*b.q+new vec4(-(a.imag|b.imag),a.imag^b.imag)+b.real*a.imag);
	public static Quaternion operator *(Quaternion a,double b)=>
	new Quaternion(a.q*b);
	public static Quaternion operator *(double a,Quaternion b)=>
	new Quaternion(a*b.q);
	public static Quaternion operator /(Quaternion a,double b)=>
	new Quaternion(a.q/b);
    public static Quaternion operator ~(Quaternion a)=>
	Mdi.conj(a);
    public static vec3 operator *(Quaternion a,vec3 b)=>
    (~a*new Quaternion(b)*a).imag;
    public static vec3 operator *(vec3 a,Quaternion b)=>
    (b*new Quaternion(a)*(~b)).imag;
	public static bool operator ==(Quaternion a,Quaternion b){
		return a.q==b.q;
	}
	public static bool operator !=(Quaternion a,Quaternion b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not Quaternion other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return HashCode.Combine(real,imag);
	}

	public override string ToString() => $"{q.x}+{q.y}i+{q.z}j+{q.w}k";
}
//線形代数
public struct vec2:IMetricVectorSpace<vec2,double>{
	public double x;
	public double y;
	public const int dim=2;
	public static vec2 zero=>0;
	public static double one=>1;
	public vec2(double X,double Y){
		x=X;
		y=Y;
	}
	public Complex z=>new Complex(this);
	public static implicit operator vec2(double a)=>
	new vec2(a,a);
	
	public static implicit operator vec2((double,double) t) =>
	new vec2(t.Item1,t.Item2);

	public static implicit operator Godot.Vector2(vec2 v) =>new Godot.Vector2((float)v.x, (float)v.y);
	public static implicit operator vec2(Complex v)=>v.z;
    public static implicit operator vec2(Godot.Vector2 v)=>new vec2(v.X, v.Y);

	public static vec2 operator +(vec2 a, vec2 b)=>
	new(a.x+b.x,a.y+b.y);
	public static vec2 operator -(vec2 a, vec2 b)=>
	new(a.x-b.x,a.y-b.y);
	public static vec2 operator -(vec2 a)=>
	new(-a.x,-a.y);
	public static vec2 operator *(vec2 a, vec2 b)=>
	new(a.x*b.x,a.y*b.y);
	public static vec2 operator *(double a, vec2 b)=>
	new(a*b.x,a*b.y);
	public static vec2 operator *(vec2 a, double b)=>
	new(a.x*b,a.y*b);
	public static vec2 operator /(vec2 a, vec2 b)=>
	new(a.x/b.x,a.y/b.y);
    public static vec2 operator /(vec2 a, double b)=>
	new(a.x/b,a.y/b);
	public static double operator |(vec2 a, vec2 b)=>
	Mdi.dot(a,b);
	public static double operator ^(vec2 a, vec2 b)=>
	Mdi.cross(a,b);
	public static bool operator ==(vec2 a,vec2 b){
		return a.x==b.x && a.y==b.y;
	}
	public static bool operator !=(vec2 a,vec2 b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not vec2 other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return HashCode.Combine(x,y);
	}
	public double length=>Math.Sqrt(x*x+y*y);
	public vec3 extend(double a)=>
		new vec3(x,y,a);
	public override string ToString() => $"({x},{y})";
}
public struct vec3:IMetricVectorSpace<vec3,double>{
	public double x;
	public double y;
	public double z;
	public const int dim=3;
	public static vec3 zero=>0;
	public static double one=>1;
	public vec3(double X,double Y,double Z){
		x=X;
		y=Y;
		z=Z;
	}
	public static implicit operator vec3(double a) =>
	new vec3(a,a,a);
    public static implicit operator Godot.Vector3(vec3 a)=>
	new Godot.Vector3((float)a.x,(float)a.y,(float)a.z);
    public static implicit operator vec3(Godot.Vector3 a)=>
	new(a.X,a.Y,a.Z);
	public static implicit operator vec3(vec2 a)=>new(a,0);
	public static implicit operator vec3((double,double,double) t)=>new(t.Item1,t.Item2,t.Item3);
	//二項演算
	public static vec3 operator +(vec3 a, vec3 b)=>new(a.x+b.x,a.y+b.y,a.z+b.z);
	public static vec3 operator -(vec3 a, vec3 b)=>new(a.x-b.x,a.y-b.y,a.z-b.z);
	public static vec3 operator -(vec3 a)=>new(-a.x,-a.y,-a.z);
	public static vec3 operator *(vec3 a, vec3 b)=>new(a.x*b.x,a.y*b.y,a.z*b.z);
	public static vec3 operator *(double a, vec3 b)=>new(a*b.x,a*b.y,a*b.z);
	public static vec3 operator *(vec3 a,double b)=>new(a.x*b,a.y*b,a.z*b);
	public static vec3 operator /(vec3 a, vec3 b)=>new(a.x/b.x,a.y/b.y,a.z/b.z);
    public static vec3 operator /(vec3 a, double b)=>new(a.x/b,a.y/b,a.z/b);
	public static vec3 operator ^(vec3 a, vec3 b)=>Mdi.cross(a,b);
	public static double operator |(vec3 a, vec3 b)=>Mdi.dot(a,b);
	public static bool operator ==(vec3 a,vec3 b){
		return a.x==b.x && a.y==b.y && a.z==b.z;
	}
	public static bool operator !=(vec3 a,vec3 b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not vec3 other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return HashCode.Combine(x,y,z);
	}
	public double length=>Math.Sqrt(x*x+y*y+z*z);
	//スウィズル
	public vec3(vec2 v,double Z){
		x=v.x;
		y=v.y;
		z=Z;
	}
	public vec3(double X,vec2 v){
		x=X;
		y=v.x;
		z=v.y;
	}
	public vec2 xy{
		get=>new vec2(x,y);
		set{
			x=value.x;
			y=value.y;
		}
	}
	public vec2 yz{
		get=>new vec2(y,z);
		set{
			y=value.x;
			z=value.y;
		}
	}
	public vec2 zx{
		get=>new vec2(z,x);
		set{
			z=value.x;
			x=value.y;
		}
	}
	public vec2 yx{
		get=>new vec2(y,x);
		set{
			y=value.x;
			x=value.y;
		}
	}
	public vec2 zy{
		get=>new vec2(z,y);
		set{
			z=value.x;
			y=value.y;
		}
	}
	public vec2 xz{
		get=>new vec2(x,z);
		set{
			x=value.x;
			z=value.y;
		}
	}
	public override string ToString() => $"({x},{y},{z})";
}
public struct vec4:IMetricVectorSpace<vec4,double>{
	public double x;
	public double y;
	public double z;
	public double w;
	public const int dim=4;
	public static vec4 zero=>0;
	public static double one=>1;
	public vec4(double X,double Y,double Z,double W){
		x=X;
		y=Y;
		z=Z;
		w=W;
	}
	public static implicit operator vec4(double a) =>
	new vec4(a,a,a,a);
	
	public static implicit operator vec4(vec2 a) =>
	new vec4(a,0,0);
	
	public static implicit operator vec4(vec3 a) =>
	new vec4(a,0);
	
	public static implicit operator vec4((double,double,double,double) t) =>
	new vec4(t.Item1,t.Item2,t.Item3,t.Item4);
	
	//二項演算
	public static vec4 operator +(vec4 a, vec4 b)=>
	new vec4(a.x+b.x,a.y+b.y,a.z+b.z,a.w+b.w);
	public static vec4 operator -(vec4 a, vec4 b)=>
	new vec4(a.x-b.x,a.y-b.y,a.z-b.z,a.w-b.w);
	public static vec4 operator -(vec4 a)=>
	new vec4(-a.x,-a.y,-a.z,-a.w);
	public static vec4 operator *(vec4 a, vec4 b) =>
	new vec4(a.x*b.x,a.y*b.y,a.z*b.z,a.w*b.w);
	public static vec4 operator *(double a, vec4 b) =>
	new vec4(a*b.x,a*b.y,a*b.z,a*b.w);
	public static vec4 operator *(vec4 a, double b) =>
	new vec4(a.x*b,a.y*b,a.z*b,a.w*b);
	public static vec4 operator /(vec4 a, vec4 b) =>
	new vec4(a.x/b.x,a.y/b.y,a.z/b.z,a.w/b.w);
    public static vec4 operator /(vec4 a, double b) =>
	new vec4(a.x/b,a.y/b,a.z/b,a.w/b);
	public static double operator |(vec4 a, vec4 b) =>
	Mdi.dot(a,b);
	public static bool operator ==(vec4 a,vec4 b){
		return a.x==b.x && a.y==b.y && a.z==b.z && a.w==b.w;
	}
	public static bool operator !=(vec4 a,vec4 b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not vec4 other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return HashCode.Combine(x,y,z,w);
	}
	public double length=>Math.Sqrt(x*x+y*y+z*z+w*w);
	//スウィズル
	public vec4(vec2 v,double a,double b){
		x=v.x;
		y=v.y;
		z=a;
		w=b;
	}
	public vec4(double a,vec2 v,double b){
		x=a;
		y=v.x;
		z=v.y;
		w=b;
	}
	public vec4(double a,double b,vec2 v){
		x=a;
		y=b;
		z=v.x;
		w=v.y;
	}
	public vec4(vec2 u,vec2 v){
		x=u.x;
		y=u.y;
		z=v.x;
		w=v.y;
	}
	public vec4(vec3 v,double a){
		x=v.x;
		y=v.y;
		z=v.z;
		w=a;
	}
	public vec4(double a,vec3 v){
		x=a;
		y=v.x;
		z=v.y;
		w=v.z;
	}
	public vec2 xy{
		get=>new vec2(x,y);
		set{
			x=value.x;
			y=value.y;
		}
	}
	public vec2 zw{
		get=>new vec2(z,w);
		set{
			z=value.x;
			w=value.y;
		}
	}
	public vec3 xyz{
		get=>new vec3(x,y,z);
		set{
			x=value.x;
			y=value.y;
			z=value.z;
		}
	}
	public vec3 yzw{
		get=>new vec3(y,z,w);
		set{
			y=value.x;
			z=value.y;
			w=value.z;
		}
	}
	public vec3 zwx{
		get=>new vec3(z,w,x);
		set{
			z=value.x;
			w=value.y;
			x=value.z;
		}
	}
	public vec3 wxy{
		get=>new vec3(w,x,y);
		set{
			w=value.x;
			x=value.y;
			y=value.z;
		}
	}
	public override string ToString() => $"({x},{y},{z},{w})";
}
public struct Vector:IMetricVectorSpace<Vector,double>{
	private readonly double[] data;
	public double[] raw=>data;
	public int dim=>data.Length;
	public static Vector zero=>0;
	public static double one=>1;
	public double length=>Mdi.sqrt(Mdi.dot(this,this));
	public Vector(int a){
		data=new double[a];
	}
	public Vector(params double[] values){
		data=values;
	}
	public double this[int i]{
		get{
			if(i<dim){
				return data[i];
			}
			return 0;
		}
		set=>data[i]=value;
	}
	public static implicit operator Vector(double x)=>
	new Vector(x);
	public static implicit operator Vector((double,double) t)=>
	new Vector(t.Item1,t.Item2);
	public static implicit operator Vector((double,double,double) t)=>
	new Vector(t.Item1,t.Item2,t.Item3);
	public static implicit operator Vector((double,double,double,double) t)=>
	new Vector(t.Item1,t.Item2,t.Item3,t.Item4);
	public static implicit operator Vector((double,double,double,double,double) t)=>
	new Vector(t.Item1,t.Item2,t.Item3,t.Item4,t.Item5);
	public static implicit operator Vector((double,double,double,double,double,double) t)=>
	new Vector(t.Item1,t.Item2,t.Item3,t.Item4,t.Item5,t.Item6);
	//演算子など
	public static Vector operator +(Vector a,Vector b){
		Vector res=new Vector(Math.Max(a.dim,b.dim));
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]+b[k];
		}
		return res;
	}
	public static Vector operator -(Vector a,Vector b){
		Vector res=new Vector(Math.Max(a.dim,b.dim));
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]-b[k];
		}
		return res;
	}
	public static Vector operator *(Vector a,Vector b){
		Vector res=new Vector(Math.Max(a.dim,b.dim));
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]*b[k];
		}
		return res;
	}
	public static Vector operator *(Vector a,double b){
		Vector res=new Vector(a.dim);
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]*b;
		}
		return res;
	}
	public static Vector operator *(double a,Vector b){
		Vector res=new Vector(b.dim);
		for(int k=0; k<res.dim; ++k){
			res[k]=a*b[k];
		}
		return res;
	}
	public static Vector operator /(Vector a,Vector b){
		Vector res=new Vector(Math.Max(a.dim,b.dim));
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]/b[k];
		}
		return res;
	}
	public static Vector operator /(Vector a,double b){
		Vector res=new Vector(a.dim);
		for(int k=0; k<res.dim; ++k){
			res[k]=a[k]/b;
		}
		return res;
	}
	public static Vector operator -(Vector a){
		Vector res=new Vector(a.dim);
		for(int k=0; k<res.dim; ++k){
			res[k]=-a[k];
		}
		return res;
	}
	public static bool operator ==(Vector a,Vector b){
		return a.data==b.data;
	}
	public static bool operator !=(Vector a,Vector b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not Vector other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return data.GetHashCode();
	}
	public static double operator |(Vector a,Vector b)=>Mdi.dot(a,b);
	public override string ToString()=>"("+string.Join(",",data)+")";
}
public struct mat2:IRing<mat2>{
	public double a,b,c,d;
	public static mat2 zero=>0;
	public static mat2 one=>1;
	public mat2(double A,double B,double C,double D){
		a=A;
		b=B;
		c=C;
		d=D;
	}
	public static implicit operator mat2(double v){
		return new mat2(v,0,0,v);
	}
	public static mat2 operator +(mat2 a,mat2 b)=>
	new mat2(a.a+b.a,a.b+b.b,a.c+b.c,a.d+b.d);
	public static mat2 operator -(mat2 a,mat2 b)=>
	new mat2(a.a-b.a,a.b-b.b,a.c-b.c,a.d-b.d);
	public static mat2 operator *(mat2 a,mat2 b)=>
	new mat2(a.a*b.a+a.b*b.c,a.a*b.b+a.b*b.d,a.c*b.a+a.d*b.c,a.c*b.b+a.d*b.d);
	public static vec2 operator *(vec2 a,mat2 b)=>
	new vec2(a.x*b.a+a.y*b.c,a.x*b.b+a.y*b.d);
	public mat2 inverse=>new mat2(d,-b,-c,a)/Mdi.det(this);
	public static mat2 operator /(mat2 a,double b){
		return new mat2(a.a/b,a.b/b,a.c/b,a.d/b);
	}
	public static mat2 operator /(mat2 a,mat2 b){
		return a*b.inverse;
	}
	public static mat2 operator *(mat2 a,double b){
		return new mat2(a.a*b,a.b*b,a.c*b,a.d*b);
	}
	public static mat2 operator *(double a,mat2 b){
		return new mat2(a*b.a,a*b.b,a*b.c,a*b.d);
	}
	public static bool operator ==(mat2 a,mat2 b){
		return a.a==b.a && a.b==b.b && a.c==b.c && a.d==b.d;
	}
	public static bool operator !=(mat2 a,mat2 b){
		return !(a==b);
	}
	public override bool Equals(object obj){
		if (obj is not mat2 other) return false;
    	return this==other;
	}
	public override int GetHashCode(){
    	return HashCode.Combine(a,b,c,d);
	}
}