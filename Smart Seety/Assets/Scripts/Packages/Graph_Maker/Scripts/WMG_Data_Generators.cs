﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IWMG_Data_Generators {
	// Generate data of the form Y = aX + b
	List<Vector2> GenLinear(int numPoints, float minX, float maxX, float a, float b);
	
	// Generate data of the form Y = aX^2 + bX + c
	List<Vector2> GenQuadratic(int numPoints, float minX, float maxX, float a, float b, float c);
	
	// Generate data of the form Y = ab^X + c
	List<Vector2> GenExponential(int numPoints, float minX, float maxX, float a, float b, float c);
	
	// Generate data of the form Y = a * log base b of X + c
	List<Vector2> GenLogarithmic(int numPoints, float minX, float maxX, float a, float b, float c);
	
	// Generate data of the form c^2 = (X - a)^2 * (Y - b)^2
	List<Vector2> GenCircular(int numPoints, float a, float b, float c);
	
	// Generate random X and Y data
	List<Vector2> GenRandomXY(int numPoints, float minX, float maxX, float minY, float maxY);
	
	// Generate random Y data
	List<Vector2> GenRandomY(int numPoints, float minX, float maxX, float minY, float maxY);
}

public class WMG_Data_Generators : IWMG_Data_Generators {

	// Generate data of the form Y = aX + b
	public List<Vector2> GenLinear(int numPoints, float minX, float maxX, float a, float b) {
		List<Vector2> results = new List<Vector2>();
		if (numPoints < 2 || maxX <= minX) return results;
		float step = (maxX - minX) / (numPoints - 1);
		for (int i = 0; i < numPoints; i++) {
			float x = i*step + minX;
			results.Add(new Vector2(x, a*x + b));
		}
		return results;
	}
	
	// Generate data of the form Y = aX^2 + bX + c
	public List<Vector2> GenQuadratic(int numPoints, float minX, float maxX, float a, float b, float c) {
		List<Vector2> results = new List<Vector2>();
		if (numPoints < 2 || maxX <= minX) return results;
		float step = (maxX - minX) / (numPoints - 1);
		for (int i = 0; i < numPoints; i++) {
			float x = i*step + minX;
			results.Add(new Vector2(x, a*x*x + b*x + c));
		}
		return results;
	}
	
	// Generate data of the form Y = ab^X + c
	public List<Vector2> GenExponential(int numPoints, float minX, float maxX, float a, float b, float c) {
		List<Vector2> results = new List<Vector2>();
		if (numPoints < 2 || maxX <= minX || b <= 0) return results;
		float step = (maxX - minX) / (numPoints - 1);
		for (int i = 0; i < numPoints; i++) {
			float x = i*step + minX;
			results.Add(new Vector2(x, a*Mathf.Pow(b,x) + c));
		}
		return results;
	}
	
	// Generate data of the form Y = a * log base b of X + c
	public List<Vector2> GenLogarithmic(int numPoints, float minX, float maxX, float a, float b, float c) {
		List<Vector2> results = new List<Vector2>();
		if (numPoints < 2 || maxX <= minX || b <= 0 || b == 1) return results;
		float step = (maxX - minX) / (numPoints - 1);
		for (int i = 0; i < numPoints; i++) {
			float x = i*step + minX;
			if (x <= 0) continue;
			results.Add(new Vector2(x, a*Mathf.Log(x,b) + c));
		}
		return results;
	}
	
	// Generate data of the form c^2 = (X - a)^2 * (Y - b)^2
	public List<Vector2> GenCircular(int numPoints, float a, float b, float c) {
		List<Vector2> results = new List<Vector2>();
		if (numPoints < 2) return results;
		float step = 360f / numPoints;
		for (int i = 0; i < numPoints; i++) {
			float deg = i*step;
			float x = c * Mathf.Cos(Mathf.Deg2Rad*deg);
			float y = c * Mathf.Sin(Mathf.Deg2Rad*deg);
			results.Add(new Vector2(x + a, y + b));
		}
		return results;
	}
	
	// Generate random X and Y data
	public List<Vector2> GenRandomXY(int numPoints, float minX, float maxX, float minY, float maxY) {
		List<Vector2> results = new List<Vector2>();
		if (maxY <= minY || maxX <= minX) return results;
		for (int i = 0; i < numPoints; i++) {
			results.Add(new Vector2(Random.Range(minX,maxX), Random.Range(minY,maxY)));
		}
		return results;
	}
	
	// Generate random Y data
	public List<Vector2> GenRandomY(int numPoints, float minX, float maxX, float minY, float maxY) {
		List<Vector2> results = new List<Vector2>();
		if (maxY <= minY || maxX <= minX) return results;
		float step = (maxX - minX) / (numPoints - 1);
		for (int i = 0; i < numPoints; i++) {
			results.Add(new Vector2(i*step + minX, Random.Range(minY,maxY)));
		}
		return results;
	}
}
