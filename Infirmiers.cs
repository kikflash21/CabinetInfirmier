namespace CabinetInfirmier;

using System.Xml.Serialization;

[XmlRoot("infirmiers", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Infirmiers
{
    [XmlElement("infirmier")] public List<Infirmier> Infirmier { get; set; }

    public Infirmiers(){}

    public Infirmiers(List<Infirmier> infirmier)
    {
        this.Infirmier = infirmier;   
    }

    public void addInfirmier(Infirmier infirmier)
    {
        Infirmier.Add(infirmier);
    }
    
    
    public string toString()
    {
        string res = "Liste d'infirmiers du cabinet : \n";
        for (int i = 0; i < Infirmier.Count; i++)
        {
            res += Infirmier[i].toString() + "\n";
        }

        return res;
    }
    
}