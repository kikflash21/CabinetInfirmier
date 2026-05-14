namespace CabinetInfirmier;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

[XmlRoot("cabinet", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Cabinet
{
    //Partie XmlReader
    public static void AnalyseGlobale(string filepath)
    {
        Console.WriteLine("Début de l'Analyse Globale du fichier :");
        XmlReader reader = XmlReader.Create(filepath);
        Console.WriteLine("On entre dans le document");
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    Console.WriteLine("-> Element '{0}' trouvé", reader.Name);
                    int attributCount =  reader.AttributeCount; 
                    Console.WriteLine("     L'element '{0}' a {1} attribut",reader.Name, attributCount);
                    break;
                
                case XmlNodeType.EndElement:
                    Console.WriteLine("-> On sort de l'element '{0}' ", reader.Name);
                    break;
                
                case XmlNodeType.Text:
                    Console.WriteLine("-> Texte '{0}' trouvé ", reader.Value);
                    break;
                
                case XmlNodeType.Attribute:
                    Console.WriteLine("-> Attribut '{0}' de valeur '{1}' trouvé", reader.Name, reader.Value);
                    break;
            }
        }
    }

    public static List<string> AnalyseNomsInfirmiers(string filepath)
    {
        XmlReader reader = XmlReader.Create(filepath);
        List<string> nomsInfirmiers = new List<string>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "infirmier")
                    {
                        //Console.WriteLine("-> Element 'infirmier' {0}", reader.Name); 
                        reader.Read();
                        reader.MoveToContent();
                        //Console.WriteLine("-> Element suivant '{0}'", reader.Name);
                        if (reader.Name == "nom") 
                        {
                             reader.Read();
                             if (reader.NodeType == XmlNodeType.Text)
                             {
                                  //Console.WriteLine("-> Nom de l'infirmier : '{0}'", reader.Value);
                                  nomsInfirmiers.Add(reader.Value);
                             }
                        }

                    }
                    break;
            } // end switch
        } // end while
        Console.WriteLine("Nombre total de noms d'infirmiers dans le cabinet : {0}", nomsInfirmiers.Count);
        return nomsInfirmiers;
    }
    
    public static List<string> AnalyseNoms(string filepath, string recherche)
    {
        XmlReader reader = XmlReader.Create(filepath);
        List<string> nomsCabinet = new List<string>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == recherche)
                    {
                        //Console.WriteLine("-> Element '{0}' trouvé ", reader.Name);
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text){
                            //Console.WriteLine("-> Nom detecté : '{0}'", reader.Value);
                                nomsCabinet.Add(reader.Value);
                        }
                    }
                    break;
            } // end switch
        } // end while
        Console.WriteLine("Nombre total de noms dans le cabinet : {0}", nomsCabinet.Count);
        return nomsCabinet;
    }

    public static int countActes(string filepath)
    {
        XmlReader reader = XmlReader.Create(filepath);
        HashSet<int> actes = new HashSet<int>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "acte" && reader.HasAttributes)
                    {
                        reader.MoveToFirstAttribute();
                        int x = 0;
                        Int32.TryParse(reader.Value, out x); //Renvoie un boolean en sortie
                        actes.Add(x);
                    }
                    break;
            }
        }
        Console.WriteLine("Nombre d'actes différents effectués (tout patient confondues) : {0}", actes.Count);
        return actes.Count;
    }
    
    //Partie serealisation du Cabinet :
    [XmlElement("nom")] public string Nom { get; set; } 
    
    [XmlElement("adresse")] public Adresse Adresse { get; set; }
    
    [XmlElement("infirmiers")] public Infirmiers Infirmiers { get; set; }
    
    [XmlElement("patients")] public Patients Patients { get; set; }
    
    public Cabinet() {}

    public Cabinet(string nom, Adresse adresse, Infirmiers infirmiers, Patients patients)
    {
        this.Nom = nom;
        this.Adresse = adresse;
        this.Patients = patients;
        this.Infirmiers = infirmiers;
    }

    public string toString()
    {
        string res = "Nom : " + Nom + "\n" + Adresse.toString() +  "\n"  + Infirmiers.toString() +  "\n" + Patients.toString();
        return res;
    }
}