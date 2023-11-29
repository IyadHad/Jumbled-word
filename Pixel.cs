using System;

public class Pixel
{
    private byte rouge;
    private byte vert;
    private byte bleu;

    /// <summary>
    /// Constructeur pixel
    /// </summary>
    /// <param name="R">couleur rouge</param>
    /// <param name="G">couleur verte</param>
    /// <param name="B">couleur bleue</param>

    public Pixel(byte R,byte G, byte B)
	{
        this.rouge = R;
        this.vert = G;
        this.bleu = B;
	}

    /// <summary>
    /// Propriété attribut rouge
    /// </summary>

    public byte Rouge
    {
        set { this.rouge = value; }
        get { return this.rouge; }
    }

    /// <summary>
    /// Propriété attribut vert
    /// </summary>

    public byte Vert
    {
        set { this.vert = value; }
        get { return this.vert; }
    }

    /// <summary>
    /// Propriété attribut bleu
    /// </summary>

    public byte Bleu
    {
        set { this.bleu = value; }
        get { return this.bleu; }
    }

    /// <summary>
    /// Retourne la description de l'intensité de chauque couleur
    /// </summary>
    /// <returns>chaîne de caractère tricolore</returns>

    public string toString()
    {
        return $"({this.rouge},{this.vert},{this.bleu})";
    }
    

}
