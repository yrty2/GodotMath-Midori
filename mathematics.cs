using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using R=System.Double;
using Z=System.Int64;
using N=System.UInt64;
using C=Complex;
using H=Quaternion;

namespace mathematics;
public class Mdi{
	//数学定数
	public const double e=Math.E;
	public const double pi=Math.PI;
	public static readonly C cone=new(1,0);
	public static readonly C czero=new(0,0);
	public static readonly C i=new(0,1);
	//初等関数やオーバーロードなど
	public static R sqrt(R a)=>
		Math.Sqrt(a);
	public static Z abs(Z a)=>
		Math.Abs(a);
	public static R abs(R a)=>
		Math.Abs(a);
	public static R abs(C a)=>
		a.z.length;
	public static R abs(H a)=>
		a.q.length;
	public static R sin(R a)=>
		Math.Sin(a);
	public static C sin(C a)=>
		(exp(i*a)-exp(-i*a))*new C(0,-1/2);
	public static R cos(R a)=>
		Math.Cos(a);
	public static C cos(C a)=>
		(exp(i*a)+exp(-i*a))/2;
	public static R tan(R a)=>
		Math.Tan(a);
	public static R asin(R a)=>
		Math.Asin(a);
	public static R acos(R a)=>
		Math.Acos(a);
	public static R atan(R a)=>
		Math.Atan(a);
	public static R sinh(R a)=>
		Math.Sinh(a);
	public static C sinh(C a)=>
		(exp(a)-exp(-a))/2;
	public static R cosh(R a)=>
		Math.Cosh(a);
	public static C cosh(C a)=>
		(exp(a)+exp(-a))/2;
	public static R tanh(R a)=>
		Math.Tanh(a);
	public static C tanh(C a){
		C ez=exp(a);
		C mez=exp(-a);
		return (ez-mez)/(ez+mez);
	}
	public static R asinh(R a)=>
		Math.Asinh(a);
	public static R acosh(R a)=>
		Math.Acosh(a);
	public static R atanh(R a)=>
		Math.Atanh(a);
	public static R atan2(R a,R b)=>
		Math.Atan2(a,b);
	public static R max(R a,R b)=>Math.Max(a,b);
	public static R min(R a,R b)=>Math.Min(a,b);
	public static float min(float a,float b)=>Math.Min(a,b);
	public static R dot<V>(V a,V b) where V:IMetricVectorSpace<V>{
		return a|b;
	}
	public static R dot(Vector a,Vector b){
		R res=0;
		for(int k=0; k<max(a.dim,b.dim); ++k){
			res+=a[k]*b[k];
		}
		return res;
	}
	public static R dot(vec2 a,vec2 b)=>
	a.x*b.x+a.y*b.y;
	public static R dot(vec3 a,vec3 b)=>
	a.x*b.x+a.y*b.y+a.z*b.z;
	public static R dot(vec4 a,vec4 b)=>
	a.x*b.x+a.y*b.y+a.z*b.z+a.w*b.w;
	public static R cross(vec2 a,vec2 b)=>
	a.x*b.y-a.y*b.x;
	public static vec3 cross(vec3 a,vec3 b)=>
	new vec3(a.y*b.z-a.z*b.y,a.z*b.x-a.x*b.z,a.x*b.y-a.y*b.x);
	public static C conj(C a)=>
	new C(a.real,-a.imag);
    public static H conj(H a)=>
	new H(a.real,-a.imag);
	public static R arg(C a)=>
	Math.Atan2(a.imag,a.real);
	public static R arg(vec2 a)=>
	Math.Atan2(a.y,a.x);
	public static Vector normalize(Vector a){
		R r=a.length;
		if(r==0){
			return new Vector(a.dim);
		}
		return a/r;
	}
	public static vec2 normalize(vec2 a){
		R r=a.length;
		if(r==0){
			return new vec2(0,0);
		}
		return a/r;
	}
	public static vec3 normalize(vec3 a){
		R r=a.length;
		if(r==0){
			return new vec3(0,0,0);
		}
		return a/r;
	}
	public static vec4 normalize(vec4 a){
		R r=a.length;
		if(r==0){
			return 0;
		}
		return a/r;
	}
	public static C normalize(C a){
		R r=abs(a);
		if(r==0){
			return 0;
		}
		return a/r;
	}
	public static H normalize(H a){
		R r=abs(a);
		if(r==0){
			return 0;
		}
		return a/r;
	}
	public static R det(mat2 a)=>a.a*a.d-a.b*a.c;
	public static R exp(R a)=>Math.Exp(a);
	public static C exp(C a)=>exp(a.real)*new C(cos(a.imag),sin(a.imag));
	public static H exp(H a){
		R abs=a.imag.length;
		R sin=Mdi.sin(abs)/abs;
		return exp(a.real)*new H(cos(abs),a.i*sin,a.j*sin,a.k*sin);
	}
	public static R floor(R a)=>Math.Floor(a);
	public static R round(R a)=>Math.Round(a);
	public static R ln(R a)=>Math.Log(a);
	public static R log(R a,R b)=>Math.Log(b)/Math.Log(a);
	public static C ln(C a)=>new C(Math.Log(dot(a.z,a.z))/2,arg(a));
	public static C poler(R radius,R theta)=>radius*new C(cos(theta),sin(theta));
	public static R pow(R a,R b)=>Math.Pow(a,b);
	public static C pow(C a,R b)=>poler(pow(abs(a),b),b*arg(a));
	public static C pow(C a,C b){
		//a^b=e^(blna)
		return exp(b*ln(a));
	}
	public static C pow(R a,C b){
		//a^b=e^(blna)
		return exp(b*ln(a));
	}
	public static R honeycomb2(N p,N q){
		if((p-2)*(q-2)==4){
			return 1;
		}
		R tan=Mdi.tan(pi/p)*Mdi.tan(pi/q);
        return sqrt(abs(tan-1)/(tan+1));
	}
	public static R honeycomb2Curvature(N p,N q){
		if((p-2)*(q-2)==4){
			return 0;
		}
		if((p-2)*(q-2)<4){
			return 1;
		}
		return -1;
	}
	public static R ceil(R x){
		R y=x%1;
		if(y==0){
			return x;
		}
		if(y<0){
			return -floor(-x);
		}
		return floor(x)+1;
	}
	//抽象的な定義
	public static R EuclideanDistance<V>(V a,V b) where V:IMetricVectorSpace<V>{
		return sqrt((a-b)|(a-b));
	}
}
//数列
public struct Seq<T>{
	public List<T> d=new List<T>();
	public T this[int i]{
		get{
			return d[i%d.Count];
		}
		set=>d[i]=value;
	}
	public Seq(){
		//空集合
		d=new List<T>();
	}
	public Seq(List<T> a){
		d=a;
	}
	public bool inset(T value){
		return d.IndexOf(value)!=-1;
	}
	public bool notinset(T value){
		return d.IndexOf(value)==-1;
	}
	public bool subset(Seq<T> a){
		List<T> res=a.d;
		foreach(T s in d){
			int i=res.IndexOf(s);
			if(i!=-1){
				res.RemoveAt(i);
			}
		}
		return res.Count==0;
	}

	public int indexOf(T value){
		return d.IndexOf(value);
	}
	public int size=>d.Count;

	public static Seq<T> operator +(Seq<T> a,T b)=>new Seq<T>(new List<T>(a.d){b});
	public static Seq<T> operator +(T a,Seq<T> b)=>new Seq<T>(new List<T>(b.d){a});
	public static Seq<T> operator +(Seq<T> a,Seq<T> b){
		List<T> res=a.d;
		foreach(T B in b.d){
			res.Add(B);
		}
		return new Seq<T>(res);
	}
	public static Seq<T> operator *(Seq<T> a,Seq<T> b){
		List<T> res=a.d;
		for(int k=0; k<res.Count; ++k){
			int i=b.d.IndexOf(res[k]);
			if(i==-1){
				res.RemoveAt(k);
				k--;
			}
		}
		return new Seq<T>(res);
	}
	public static Seq<T> operator -(Seq<T> a,T b){
		List<T> res=a.d;
		res.Remove(b);
		return new Seq<T>(res);
	}
	public static Seq<T> operator /(Seq<T> a,T b){
		List<T> res=a.d;
		for(int k=0; k<res.Count; ++k){
			if(EqualityComparer<T>.Default.Equals(a.d[k],b)){
				res.RemoveAt(k);
				k--;
			}
		}
		return new Seq<T>(res);
	}
	public static Seq<T> operator /(Seq<T> a,Seq<T> b){
		List<T> res=a.d;
		for(int k=0; k<res.Count; ++k){
			if(b.d.IndexOf(a.d[k])!=-1){
				a.d.RemoveAt(k);
				k--;
			}
		}
		return new Seq<T>(res);
	}
	public static Seq<T> operator |(Seq<T> a,Func<T,bool> b){
		Seq<T> res=new Seq<T>();
		foreach(T s in a.d){
			if(b(s)){
				res+=s;//要素の追加
			}
		}
		return res;
	}
}
//集合
public struct Set<T>{
	public IEnumerator<T> GetEnumerator(){
        foreach (var x in d){
            yield return x; // yield return で順番に返す
		}
    }
	public HashSet<T> d;
	public Set(IEnumerable<T> items){
        d =new HashSet<T>(items);
    }
	public Set(){
		//空集合
		d=new HashSet<T>();
	}
	public Set(List<T> a){
		d=new HashSet<T>(a);
	}
	public Set(HashSet<T> a){
		d=a;
	}
	public bool inset(T value){
		return d.Contains(value);
	}
	public bool notinset(T value){
		return !inset(value);
	}
	public bool subset(Set<T> a){
		return d.IsSupersetOf(a.d);
	}
	public Set<T> copy(){
		Set<T> res=new Set<T>();
		foreach(var p in d){
			res+=p;
		}
		return res;
	}
	public int size=>d.Count;
	public static bool operator ==(Set<T> a,Set<T> b){
		return a.d.SetEquals(b.d);
	}
	public static bool operator !=(Set<T> a,Set<T> b){
		return !a.d.SetEquals(b.d);
	}
	public override bool Equals(object obj){
    	if (obj is not Set<T> other) return false;
    	return d.SetEquals(other.d);
	}
	public override int GetHashCode(){
    	int hash=17;
    	foreach (var item in d){
        	hash=hash*31+(item?.GetHashCode()??0);
		}
    	return hash;
	}
	public static bool operator <(Set<T> a,Set<T> b){
		return b.subset(a);
	}
	public static bool operator >(Set<T> a,Set<T> b){
		return a.subset(b);
	}
	public static bool operator >(T a,Set<T> b){
		return false;
	}
	public static bool operator <(T a,Set<T> b){
		return b.inset(a);
	}
	public static bool operator >(Set<T> a,T b){
		return a.inset(b);
	}
	public static bool operator <(Set<T> a,T b){
		return false;
	}
	public static Set<T> operator +(Set<T> a,T b){
		Set<T> res=new Set<T>(a.d);
        res.d.Add(b);
		return res;
	}
	public static Set<T> operator +(T a,Set<T> b){
		Set<T> res=new Set<T>(b.d);
        res.d.Add(a);
		return res;
	}
	public static Set<T> operator +(Set<T> a,Set<T> b){
		Set<T> res=new Set<T>(a.d);
        res.d.UnionWith(b.d);
		return res;
	}
	public static Set<T> operator *(Set<T> a,Set<T> b){
		Set<T> res=new Set<T>(a.d);
        res.d.IntersectWith(b.d);
		return res;
	}
	public static Set<T> operator /(Set<T> a,T b){
		Set<T> res=new Set<T>(a.d);
        res.d.Remove(b);
		return res;
	}
	public static Set<T> operator /(Set<T> a,Set<T> b){
		Set<T> res=new Set<T>(a.d);
        res.d.ExceptWith(b.d);
		return res;
	}
	public static Set<T> operator |(Set<T> a,Func<T,bool> b){
		Set<T> res=new Set<T>();
		foreach(T s in a.d){
			if(b(s)){
				res+=s;//要素の追加
			}
		}
		return res;
	}
}
//対象群
public class S:IGroup<S>{
    public int[] p;
    public int size=>p.Length;
    public S(int[] arr){
        p=(int[])arr.Clone();
    }
    public S identity(){
        int[] res=new int[size];
        for(int i=0; i<size; i++){
			res[i]=i;
		}
        return new S(res);
    }
    public static S operator *(S a,S b){
		var res=new int[a.size];
        for(int i=0; i<a.size; i++){
            res[i]=a.p[b.p[i]];
        }
        return new S(res);
	}
    public S inverse(){
        var res=new int[size];
        for(int i=0; i<size; i++){
            res[p[i]]=i;
        }
        return new S(res);
    }
    public override string ToString() {
        return $"({string.Join(",", p)})";
    }
}
public abstract class TopologicalSpace<X>{
	public Set<X> point=new Set<X>();
    public Set<Set<X>> topology=new Set<Set<X>>();
	//topology
    public TopologicalSpace(Set<X> points){
        point=points.copy();
    }
    public void AddTopolygy(Set<X> subset){
        var set=subset.copy();
        if(!(set<point)){
            throw new ArgumentException("Illegal Input");
		}
        topology+=set;
    }
    public bool isOpen(Set<X> subset){
        return topology.d.Any(os=>os==subset);
    }
}
public abstract class HausdorffSpace<X>:TopologicalSpace<X>{
	public HausdorffSpace(Set<X> points):base(points){
    }
}
public abstract class MetricSpace<X>:HausdorffSpace<X>{
    public Func<X,X,double> dist;
    public MetricSpace(Set<X> points,Func<X,X,double> distance):base(points){
        dist=distance;
    }
	//ε-近傍
    public Set<X> neighbor(X a,double epsilon){
        return point|(x=>dist(x,a)<epsilon);
    }
	public bool isOpenAt(Set<X> A,X a,double epsilon){
		if(A<point){
			if(neighbor(a,epsilon)<A){
				return true;
			}
		}
		return false;
	}
	public bool isOpen(Set<X> A,double epsilon){
		if(A<point){
			foreach(var a in A){
				if(!(neighbor(a,epsilon)<A)){
					return false;
				}
				return true;
			}
		}
		return false;
	}
}