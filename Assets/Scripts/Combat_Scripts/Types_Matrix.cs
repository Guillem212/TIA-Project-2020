using UnityEngine;

public class Types_Matrix
{
    ///<summary>
    ///The rows are the attacking Type, and the columns are the defending Type. 
    ///The attacking Type is the Type of the attack. 
    ///The defending Type is the Type of the Pokemon.
    ///</summary>
    public float[,] types_Matrix;

    public Types_Matrix() =>
        //types_Matrix = new int[18,18];
        types_Matrix = new float[,] {
        //   BUG,DARK,DRAG,ELEC,FAIR,FIGH,FIRE,FLYI,GHOS,GRAS,GROU, ICE,NORM,POIS,PSYC,ROCK,STEE,WATER
            {  1,   2,   1,   1, .5f, .5f, .5f, .5f, .5f,   2,   1,   1,   1, .5f,   2,   1, .5f,   1}, //BUG
            {  1, .5f,   1,   1, .5f, .5f,   1,   1,   2,   1,   1,   1,   1,   1,   2,   1,   1,   1}, //DARK
            {  1,   1,   2,   1,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1, .5f,   1}, //DRAGON
            {  1,   1, .5f, .5f,   1,   1,   1,   2,   1, .5f,   0,   1,   1,   1,   1,   1,   1,   2}, //ELECTRIC
            {  1,   2,   2,   1,   1,   2, .5f,   1,   1,   1,   1,   1,   1, .5f,   1,   1, .5f,   1}, //FAIRY
            {.5f,   2,   1,   1, .5f,   1, .5f,   0,   1,   1,   2,   2, .5f, .5f,   2,   2,   1,   1}, //FIGHTING
            {  2,   1, .5f,   1,   1,   1, .5f,   1,   1,   2,   1,   2,   1,   1,   1, .5f,   2, .5f}, //FIRE
            {  2,   1,   1, .5f,   1,   2,   1,   1,   1,   2,   1,   1,   1,   1,   1, .5f, .5f,   1}, //FLYING
            {  1, .5f,   1,   1,   1,   1,   1,   1,   2,   1,   1,   1,   0,   1,   2,   1,   1,   1}, //GHOST
            {.5f,   1, .5f,   1,   1,   1, .5f, .5f,   1, .5f,   2,   1,   1, .5f,   1,   2, .5f,   2}, //GRASS
            {.5f,   1,   1,   2,   1,   1,   2,   0,   1, .5f,   1,   1,   1,   2,   1,   2,   2,   1}, //GROUND
            {  1,   1,   2,   1,   1,   1, .5f,   2,   1,   2,   2, .5f,   1,   1,   1,   1, .5f, .5f}, //ICE
            {  1,   1,   1,   1,   1,   1,   1,   1,   0,   1,   1,   1,   1,   1,   1, .5f, .5f,   1}, //NORMAL
            {  1,   1,   1,   1,   2,   1,   1,   1, .5f,   2, .5f,   1,   1, .5f,   1, .5f,   0,   1}, //POISON
            {  1,   0,   1,   1,   1,   2,   1,   1,   1,   1,   1,   1,   1,   2, .5f,   1, .5f,   1}, //PSYCHIC
            {  2,   1,   1,   1,   1, .5f,   2,   2,   1,   1, .5f,   2,   1,   1,   1,   1, .5f,   1}, //ROCK
            {  1,   1,   1, .5f,   2,   1, .5f,   1,   1,   1,   1,   2,   1,   1,   1,   2, .5f, .5f}, //STEEL
            {  1,   1, .5f,   1,   1,   1,   2,   1,   1, .5f,   2,   1,   1,   1,   1,   2,   1, .5f}  //WATER
        };
    
    public void Print(float[,] types_Matrix){
        for (int i = 0; i < types_Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < types_Matrix.GetLength(1); j++)
            {
                Debug.Log(types_Matrix[i, j]);
            }
        }
    }
}

