using System;
using UnityEngine;
using System.Collections;


//We use the TileVec to specify the Position of Tiles to not confuse them with 3D Vectors
public struct TileVec: IComparable<TileVec> {

	private int x;
	private int y;
	//private Vector2 vector;
	public int X { get{return this.x;}}
	public int Y { get{return this.y;}}

	public TileVec(int x, int y) {
		this.x = x;
		this.y = y;
		//this.vector = new Vector2(x,y);
	}

	public TileVec(Vector2 vec) {
		this.x = (int) vec.x;
		this.y = (int) vec.y;
		//this.vector = new Vector2(x,y);
	}

	public override string ToString() {
		return "x= "+this.x+" y="+this.y;
	}
	public int CompareTo(TileVec other)
    {
        return Math.Abs(x-other.X) + Math.Abs(y-other.Y);
    }
}
