using Godot;
using System;
using mathematics;

public class ProjectiveGeometry{
	public static vec2 perspective(vec3 a)=>a.xy/a.z;
	public static vec3 perspective(vec4 a)=>a.xyz/a.w;
	public static Vector perspective(Vector a)=>ProjectiveGeometry.orthographic(a)/a[a.dim-1];
	public static Vector orthographic(Vector a)=>new Vector(a.raw[0..(a.dim-1)]);
}
public struct Geometry{
	public double curvature;
	public int dim;
	public Geometry(double Curvature,int dimension){
		curvature=Curvature;
		dim=dimension;
	}
	public Vector translate(Vector p,Vector q){
		if(curvature==0){
			return p+q;
		}
		double pp=p|p;
		double pq=p|q;
		double qq=q|q;
		return (q*(1+curvature*pp)+p*(1-curvature*(qq+2*pq)))/(1+curvature*(curvature*pp*qq-2*pq));
	}
	public override string ToString(){
		string geo="Euclidean";
		string adt="";
		if(curvature<0){
			geo="Hyperbolic";
			if(curvature!=-1){
				adt=$"({curvature})";
			}
		}
		if(curvature>0){
			geo="Spherical";
			if(curvature!=1){
				adt=$"({curvature})";
			}
		}
		return $"{dim}-dimensional {geo} Geometry{adt}";
	}
}
//for 2 dimentional hyperbolic geometry
/*public class geoh2{
	public static C translate(C p,C q){
		return (p+q)/(p*~q+1);
	}
	public static C scale(C a,double s){
		if(a.real==0 && a.imag==0){
			return 0;
		}
		return a*Mdi.tanh(s*Mdi.atanh(a.z.length))/a.z.length;
	}
	public static Complex midpoint(Complex p,Complex q){
		return translate(scale(translate(p,-q),0.5),q);
	}
	public static double length(Complex p){
		return 2*Mdi.atanh(Mdi.abs(p));
	}
	public static double distance(Complex p,Complex q){
		return length(translate(p,-q));
	}
	public static Complex reflection(Complex p,Complex q,Complex c){
		Complex pc=translate(p,-c);
		Complex qc=translate(q,-c);
		return translate(translate(pc-2*qc*(pc.z|qc.z)/(qc.z|qc.z),-c),scale(q,2));
	}
	public static vec2[] reflectionArray(vec2[] a,Complex q,Complex c){
		vec2[] v=new vec2[a.Length];
		for(uint k=0; k<a.Length; ++k){
			v[k]=reflection(a[k],q,c);
		}
		return v;
	}
}
public struct MinkowskiSpace{
}
public struct h2mat{
	public Complex a,b;
	public h2mat(Complex A,Complex B){
		a=A;
		b=B;
	}
	public h2mat inverse=>new h2mat(~a,-b);
	public h2mat rotation(double t){
		return new h2mat(a*Mdi.poler(1,t/2),b*Mdi.poler(1,t/2));
	}
	public h2mat rotation2(double t){
		return new h2mat(a*Mdi.poler(1,t/2),b*Mdi.poler(1,-t/2));
	}
	//これで回転もやっちゃおう
	public static h2mat operator +(h2mat a,h2mat b)=>
	new h2mat(a.a+b.a,a.b+b.b);
	public static h2mat operator -(h2mat a,h2mat b)=>
	new h2mat(a.a-b.a,a.b-b.b);
	public static h2mat operator *(h2mat a,h2mat b)=>
	new h2mat(a.a*b.a+a.b*~b.b,a.a*b.b+a.b*~b.a);
	public static h2mat operator *(h2mat a,double b)=>
	new h2mat(a.a*b,a.b*b);
	public static h2mat operator *(double b,h2mat a)=>
	new h2mat(b*a.a,b*a.b);
	public static h2mat operator /(h2mat a,double b)=>
	new h2mat(a.a/b,a.b/b);
	//Möbius変換
	public static Complex operator *(Complex a,h2mat b)=>(b.a*a+b.b)/(~b.b*a+~b.a);
}*/