/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

public enum PieceType {King, Queen, Bishop, Knight, Rook, Pawn};

// 所有棋子的父类
public abstract class Piece : MonoBehaviour
{
    public PieceType type;

	public string name;
	public int attack;
	public int defense;
	public int health;
	public int maxHealth;
	public int is_range;

	public HealthBar healthBar;

	// 新的通用特征，比如攻防血，加在这里
	// 水平和斜着两种通用移动方法，特殊移动方法如马在Piece的子类里
    protected Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);

	public void Awake()
	{
		setup_attributes();
	}

	public virtual void setup_attributes()
	{
		name = "Soldiers";
		is_range = 0;
		attack = 2;
		defense = 1;
		health = 2;
		maxHealth = health;
	}

	public void attack_piece(Piece p2)
	{
		healthBar.SetMaxHealth(maxHealth);
		p2.healthBar.SetMaxHealth(p2.maxHealth);

		p2.health = p2.health - attack + p2.defense;
		if (is_range == 0)
		{
			health =  health - p2.attack + defense;
		}

		if (health < 1)
		{
			health=1;
		}
		
		if (p2.health < 0)
		{
			p2.health = 0;
		}

		healthBar.SetHealth(health);
		p2.healthBar.SetHealth(p2.health);
	}
}
