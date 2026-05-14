namespace CabinetInfirmier;

using System.Text;
using System.Xml;

//Parseur DOM
public class CabinetDOM
{
    private XmlDocument doc;
    private XmlNode root;
    private XmlNamespaceManager nsmgr;
    
    public CabinetDOM(string filepath)
    {
        doc = new XmlDocument();
        doc.PreserveWhitespace = false; // methode pour l'indentation
        doc.Load(filepath);
        root = doc.DocumentElement; //recuperer la racine
        nsmgr = new XmlNamespaceManager(doc.NameTable); //namespace
        nsmgr.AddNamespace(root.Prefix, root.NamespaceURI);
    }

    public XmlNodeList getXpath(string nPrefix, string nsURI, string expressionXpath)
    {
        nsmgr.AddNamespace(nPrefix, nsURI);
        return root.SelectNodes(expressionXpath, nsmgr);
    }

    public int count(string elementName)
    {
        int count = 0;
        XmlElement rootElt = (XmlElement) root;
        XmlNodeList recherche = rootElt.GetElementsByTagName(elementName);
        return recherche.Count;
    }
    
    // Ici on suppose qu'une adresse est considéré comme complète si elle contient exactement 5 éléments (étage, numéro, rue, codePostal et ville).
    private bool adresseEstCompltete(XmlNode adresssNode)
    {
        bool res = true;
        var elemFils = adresssNode.ChildNodes;
        int countElem = elemFils.Count;
        return (countElem == 5);
    }
    
    public bool adresseCabinetEstComplete() 
    {
        string expression = "//cab:cabinet/cab:adresse";
        XmlNodeList node = getXpath("cab", "http://www.univ-grenoble-alpes.fr/l3miage/medical", expression);
        bool res = adresseEstCompltete(node[0]);
        return res;
    }

    public bool adressePatientEstComplete(string nomPatient)
    {
        string expression = "//cab:patient[cab:nom/text()='" + nomPatient + "']/cab:adresse";
        XmlNodeList nodes = getXpathPerso(expression);
        bool res = false;
        if (nodes.Count >= 1)
        { 
            res = adresseEstCompltete(nodes[0]);
        }
        return res;
    }

    private string getExpressionDansNss(string nomPatient, string elementName)
    {
        string expr = "//cab:patient[cab:nom/text()='" + nomPatient + "']/cab:" + elementName; 
        return expr;
    }

    private XmlNodeList getXpathPerso(string expression) //Fonction spécifique de getXPath() pour utilisation plus pratique 
    {
        XmlNodeList node = getXpath("cab", "http://www.univ-grenoble-alpes.fr/l3miage/medical", expression);
        return node;
    }
    
    public bool nssValide(string nomPatient) 
    {
        bool res = true;
        string expression = "//cab:patient[cab:nom/text()='" + nomPatient + "']/cab:numéro"; 
        XmlNodeList noeudlist = getXpathPerso(expression);
        if (noeudlist.Count != 0)
        {
            XmlNode nssNode = noeudlist[0];
            string nss =  nssNode.InnerText;
            int tailleNss = nss.Length;
            char sexe = 'N';
            if (nss[0] == '1')
            {
                sexe = 'M';
            }
            else if (nss[0] == '2')
            {
                sexe = 'F';
            }
            string anneeNaiss = "" + nss[1] + nss[2];
            string moisNaiss = "" + nss[3] + nss[4];
            
            var sexeNss  = getXpathPerso(getExpressionDansNss(nomPatient, "sexe"))[0].InnerText[0];
            var naissance  =  getXpathPerso(getExpressionDansNss(nomPatient, "naissance"))[0].InnerText;
            string anneeNaissNss = "" + naissance[2] + naissance[3];
            string moisNaissNss = "" + naissance[5] + naissance[6];
            
            if (tailleNss != 15 || (sexe != sexeNss) || !anneeNaiss.Equals(anneeNaissNss) ||
                !moisNaiss.Equals(moisNaissNss))
            {
                res = false;
            }
            
        }
        else
        {
            res = false;
        }
        
        return res;
    }
    
    //Partie Modification de l'arbre DOM et de l'instance XML
    
    private XmlElement MakeInfirmier(string nom, string prenom)
    {
        XmlElement infirmierElt = doc.CreateElement(root.Prefix, "infirmier", root.NamespaceURI);
        int coundId = count("infirmier") + 1;

        string id = "";
        if (coundId <= 9)
        {
            id = "00" + coundId; 
        }
        else if (coundId <= 99)
        {
            id = "0" + coundId;
        }
        else
        {
            id = "" + coundId;
        }
        
        string photo = prenom.ToLower() + ".png"; 
        
        infirmierElt.SetAttribute("id", id);
        
        XmlElement nomElt = doc.CreateElement(root.Prefix, "nom", root.NamespaceURI);
        XmlText nomTxt = doc.CreateTextNode(nom);
        nomElt.AppendChild(nomTxt);
        
        XmlElement prenomElt = doc.CreateElement(root.Prefix, "prénom", root.NamespaceURI);
        XmlText prenomTxt = doc.CreateTextNode(prenom);
        prenomElt.AppendChild(prenomTxt);
        
        XmlElement photoElt = doc.CreateElement(root.Prefix, "photo", root.NamespaceURI);
        XmlText photoTxt = doc.CreateTextNode(photo);
        photoElt.AppendChild(photoTxt);
        
        // ordre a respecter
        infirmierElt.AppendChild(nomElt);
        infirmierElt.AppendChild(prenomElt);
        infirmierElt.AppendChild(photoElt);
        
        return infirmierElt;
    }

    public void addInfirmier(string nom, string prenom)
    {
        XmlElement newInfirmierElt = MakeInfirmier(nom, prenom);
        XmlElement rootElt = (XmlElement) root;
        var nodeInfirmiers = rootElt.GetElementsByTagName("infirmiers").Item(0); //recuperer le node Infirmiers
        nodeInfirmiers.AppendChild(newInfirmierElt);
        
        string chemin = "../../../data/xml/newCabinetDOM.xml";
        //doc.Save(chemin); //Modification de l'instance XML (nouveau doc : newCabinetDOM.xml)
        
        XmlTextWriter writer = new XmlTextWriter(chemin, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 4;
        doc.Save(writer);
    }

    private XmlElement MakePatient(string nom, string prenom, string dateNaissance, string nss, Adresse adresse)
    {
        XmlElement patientElt = doc.CreateElement(root.Prefix, "patient", root.NamespaceURI);
        
        XmlElement nomElt = doc.CreateElement(root.Prefix, "nom", root.NamespaceURI);
        XmlText nomTxt = doc.CreateTextNode(nom);
        nomElt.AppendChild(nomTxt);
        
        XmlElement prenomElt = doc.CreateElement(root.Prefix, "prénom", root.NamespaceURI);
        XmlText prenomTxt = doc.CreateTextNode(prenom);
        prenomElt.AppendChild(prenomTxt);

        string sexe = "";
        if (nss[0] == '1')
        {
            sexe = "M";
        }
        else if (nss[0] == '2')
        {
            sexe = "F";
        }
        XmlElement sexeElt =  doc.CreateElement(root.Prefix, "sexe", root.NamespaceURI);
        XmlText sexeTxt = doc.CreateTextNode(sexe);
        sexeElt.AppendChild(sexeTxt);

        XmlElement naissElt = doc.CreateElement(root.Prefix, "naissance", root.NamespaceURI);
        XmlText naissTxt = doc.CreateTextNode(dateNaissance);
        naissElt.AppendChild(naissTxt);
        
        XmlElement nssElt = doc.CreateElement(root.Prefix, "numéro", root.NamespaceURI);
        XmlText nssTxt = doc.CreateTextNode(nss);
        nssElt.AppendChild(nssTxt);
        
        XmlElement adresseElt = doc.CreateElement(root.Prefix, "adresse", root.NamespaceURI);
        if (adresse.Etage != 0)
        {
            XmlElement etageElt = doc.CreateElement(root.Prefix, "étage", root.NamespaceURI);
            XmlText etageTxt = doc.CreateTextNode(adresse.Etage.ToString());
            etageElt.AppendChild(etageTxt);
            adresseElt.AppendChild(etageElt);
        }

        if (adresse.Numero != 0)
        {
            XmlElement numeroElt = doc.CreateElement(root.Prefix, "numéro", root.NamespaceURI);
            XmlText numeroTxt = doc.CreateTextNode(adresse.Numero.ToString());
            numeroElt.AppendChild(numeroTxt);
            adresseElt.AppendChild(numeroElt);
        }
        
        XmlElement rueElt =  doc.CreateElement(root.Prefix, "rue", root.NamespaceURI);
        XmlText rueTxt = doc.CreateTextNode(adresse.Rue);
        rueElt.AppendChild(rueTxt);
        adresseElt.AppendChild(rueElt);
        
        XmlElement codePostElt = doc.CreateElement(root.Prefix, "codePostal", root.NamespaceURI);
        XmlText codePostTxt = doc.CreateTextNode(adresse.CodePostal.ToString());
        codePostElt.AppendChild(codePostTxt);
        adresseElt.AppendChild(codePostElt);
        
        XmlElement villeElt = doc.CreateElement(root.Prefix, "ville", root.NamespaceURI);
        XmlText villeTxt = doc.CreateTextNode(adresse.Ville);
        villeElt.AppendChild(villeTxt);
        adresseElt.AppendChild(villeElt);
        
        patientElt.AppendChild(nomElt);
        patientElt.AppendChild(prenomElt);
        patientElt.AppendChild(sexeElt);
        patientElt.AppendChild(naissElt);
        patientElt.AppendChild(nssElt);
        patientElt.AppendChild(adresseElt);
        
        return patientElt;
    }
    
    public void addPatient(string nom, string prenom, string dateNaissance, string nss, Adresse adresse)
    {
        XmlElement newPatientElt = MakePatient(nom, prenom, dateNaissance, nss, adresse);
        XmlElement rootElt = (XmlElement) root;
        var nodePatients = rootElt.GetElementsByTagName("patients").Item(0);
        nodePatients.AppendChild(newPatientElt);
        
        string chemin = "../../../data/xml/newCabinetDOM.xml";
        doc.Save(chemin); //Modification de l'instance XML (nouveau doc : newCabinetDOM.xml)
    }

    private XmlElement makeVisite(string date, int intervenant, List<Acte> listActeId)
    {
        XmlElement visiteElt = doc.CreateElement("visite", root.NamespaceURI);

        visiteElt.SetAttribute("date", date);
        visiteElt.SetAttribute("intervenant", "00" + intervenant);
        
        foreach (Acte acteId in listActeId)
        {
            XmlElement acteElt = doc.CreateElement("acte", root.NamespaceURI);
            acteElt.SetAttribute("id", acteId.Id);
            visiteElt.AppendChild(acteElt);
        }
        return visiteElt;
    }

    public void addVisite(string date, int intervenant, List<Acte> listActeId, string nomPatient)
    {
        XmlElement newVisiteElt = makeVisite(date, intervenant, listActeId);

        string expression = "//cab:patient[cab:nom/text() = '" + nomPatient + "']"; 
            
        XmlNodeList patientNode = getXpath("cab", "http://www.univ-grenoble-alpes.fr/l3miage/medical", expression);

        if (patientNode.Count != 0)
        {
            patientNode[0].AppendChild(newVisiteElt);
            string chemin = "../../../data/xml/newCabinetDOM.xml"; 
            //doc.Save(chemin); //Modification de l'instance XML (nouveau doc : newCabinetDOM.xml)
            
            XmlTextWriter writer = new XmlTextWriter(chemin, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            doc.Save(writer);
        }
    }
}