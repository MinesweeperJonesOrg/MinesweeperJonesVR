using UnityEngine;
using System.Collections.Generic;

public class PrepareMines : MonoBehaviour
{
    #region fields
    private List<Mine> m_AllMines;
    private Mine[,] m_Minefield;

    public int sizeX = 10;
    public int sizeY = 10;
    public uint mineCount = 10;
    #endregion

    #region Properties
    //returns all the mines ignoring "free" squares
    public List<Mine> Mines
    {
        get
        {
            return m_AllMines;
        }
    }

    //returns a 2D array of mines, free spaces are nulls
    public Mine[,] Minefield
    {
        get
        {
            return m_Minefield;
        }
    }
    #endregion

    // Use this for initialization
    //clarify which side is X and which side is Y
    void Start ()
    {
        if (sizeX < 2 || sizeY < 2 || sizeX > 255 || sizeY > 255)
            return; //size too small
        if (mineCount > (sizeX * sizeY) / 2)
            return; //too many mines

        //unity also has a class called "Random". haha. fuck...
        System.Random rng = new System.Random();
        m_AllMines = new List<Mine>();

        //lay mines
        uint minesPlanted = 0;
        uint loopCount = 0;
        int posX = 0;
        int posY = 0;
        while (loopCount < 10000 && minesPlanted < mineCount)
        {
            posX = rng.Next(sizeX + 1);
            posY = rng.Next(sizeY + 1);
            Mine newMine = new Mine(posX, posY);
            if (this.AddMine(newMine))
                minesPlanted++;
            loopCount++;
            //ToDo: this method is shitty for high mine counts! must be optimized later. For now I restrict max mine number to half of the total field size to avoid larger problems
        }
        System.Diagnostics.Debug.Assert(minesPlanted != mineCount, "Could not place all mines, script is not optimized for high mine count yet");

        //while a list of mines is nice, now we also project these objects onto a 2-dimensional array that mimics our playing field
        Mine[,] newMinefield = new Mine[sizeX, sizeY];
        foreach(Mine m in m_AllMines)
        {
            newMinefield[m.GetPosX(), m.GetPosY()] = m;
        }
        m_Minefield = newMinefield;
    }

    // Update is called once per frame
    void Update ()
    {
        return; //nothing to update yet
    }

    public bool AddMine(Mine myMine)
    {
        bool retval = false;
        if (myMine != null && m_AllMines != null && !m_AllMines.Contains(myMine))   //if position already taken, dont do anything
        {
            m_AllMines.Add(myMine);
            retval = true;
        }
        return retval;
    }
}

#region Mine Class
public class Mine:System.IEquatable<Mine>
{
    #region fields
    int m_ID;   //hash value, for quick comparison of mines. I.e. see if theres already a mine on that position
    int m_PosX;
    int m_PosY;
    //add further "special" mine properties later?
    #endregion

    #region ctor
    public Mine(int x, int y)
    {
        if (x < 0 || y < 0 || x > 255 || y > 255)
            throw new System.ArgumentException("provided invalid position for new mine");
        m_PosX = x;
        m_PosY = y;

        //build the hash - this needs to be extended if more mine properties are added
        m_ID = (m_PosX.GetHashCode() ^ (IntExtender.RotateLeft(m_PosY.GetHashCode(), 8)));
    }
    #endregion

    #region properties
    public int GetPosX()
    {
        return m_PosX;
    }
    public int GetPosY()
    {
        return m_PosY;
    }
    #endregion

    #region IEquatable members
    public override string ToString()
    {
        return "ID" + m_ID.ToString();
    }

    public override int GetHashCode()
    {
        return m_ID;
    }

    public bool Equals(Mine otherMine)
    {
        if (otherMine == null) return false;

        return (this.m_ID == otherMine.m_ID);
    }
    #endregion


}
#endregion

public static class IntExtender
{
    public static int RotateLeft(int value, int count)
    {
        return (value << count) | (value >> (32 - count));
    }
}