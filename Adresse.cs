namespace CabinetInfirmier;

using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlRoot("adresse", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Adresse
{
    [XmlIgnore]
    private string patternEntierPositif = @"^[0-9]+$";
    private string patternCodePostale =  @"^\d{5}$";

    private int _etage;
    [XmlElement("étage")]
    public int Etage
    {
        get => _etage;
        set
        {
            if (Regex.IsMatch(value.ToString(), patternEntierPositif))
                _etage = value;
            else
                throw new Exception("Un etage d'adresse doit etre un entier positif.");
        }
    }
    
    private int _numero;
    [XmlElement("numéro")] 
    public int Numero
    {
        get => _numero;
        set
        {
            if (Regex.IsMatch(value.ToString(), patternEntierPositif))
                _numero = value;
            else
                throw new Exception("Un numero d'adresse doit etre un entier positif.");
        }
    }

    private string _codePostal;
    [XmlElement("rue")]
    public string Rue { get; set; }
    
    [XmlElement("codePostal")]
    public string CodePostal
    {
        get => _codePostal;
        set
        {
            if (Regex.IsMatch(value, patternCodePostale))
                _codePostal = value;
            else
                throw new Exception("Un code postale d'adresse doit etre un entier à 5 chiffres.");
        }
    }
    
    [XmlElement("ville")]
    public string Ville { get; set; }
    
    public Adresse() { }

    public Adresse(int etage, int numero, string rue, string codePostal, string ville)
    {
        Numero = numero;
        Rue = rue;
        CodePostal = codePostal;
        Ville = ville;
        Etage = etage;
    }
    public string toString()
    {
        string res = "Adresse : Etage : " + Etage + "\nNumero : " + Numero + "\nrue : " + Rue + "\ncodePostal : "  + CodePostal + "\nville : " + Ville; 
        return res;
    }
}