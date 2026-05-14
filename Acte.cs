namespace CabinetInfirmier;

using System.Xml.Serialization;
using System.Text.RegularExpressions;

[XmlRoot("acte", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Acte
{
    private string patternEntierPositif = @"^[0-9]+$";

    private string id;
    [XmlAttribute("id")]
    public string Id
    {
        get => id;
        set
        {
            if (Regex.IsMatch(value, patternEntierPositif))
                id = value;
            else
                throw new Exception("Acte id doit etre un entier positif");
        }
    }
    
    public Acte(){}

    public Acte(string id)
    {
        Id = id;
    }

    public string toString()
    {
        string res = "Acte id = " +  Id + "\n";
        return res;
    }
}