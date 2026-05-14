namespace CabinetInfirmier;

using System.Xml;
using System.Xml.Serialization;
class Program
{
    public static async Task Main(string[] args)
    {
        
        //-------------- Validation du schéma avec l'instance des fichiers Cabinet --------------
        XMLUtils.ValidateXmlFileAsync("../../../data/xml/cabinet.xml",("http://www.univ-grenoble-alpes.fr/l3miage/medical", "../../../data/xsd/cabinet.xsd"), ("http://www.univ-grenoble-alpes.fr/l3miage/actes", "../../../data/xsd/actes.xsd"));
        
        
        //-------------- Xslt transformation --------------
        XMLUtils.XslTransform("../../../data/xml/cabinet.xml", "../../../data/xslt/cabinet.xsl", "../../../data/html/PageInfirmier.html"); 
        
        XMLUtils.XslTransform("../../../data/xml/patient.xml", "../../../data/xslt/patient_html.xsl", "../../../data/html/PagePatient.html"); 
        
        
        //-------------- pageInfirmiere avec parametre (id="002") --------------
        XMLUtils.XslTransformParam("../../../data/xml/cabinet.xml", "../../../data/xslt/cabinet.xsl", "../../../data/html/PageInfirmier_param.html", "destinedId", "002");
        
        
        
        //-------------- Partie parseur XmlReader -------------- 
        Cabinet.AnalyseGlobale("./data/xml/cabinet.xml"); 

        Cabinet.AnalyseNomsInfirmiers("./data/xml/cabinet.xml");
        
        Cabinet.AnalyseNoms("./data/xml/cabinet.xml", "nom");

        //Compte le nombre d'actes differents necessaires, tout patient confondus.
        int actes = Cabinet.countActes("./data/xml/cabinet.xml");
        

        //-------------- Partie parseur DOM --------------
        string filename = "./data/xml/cabinet.xml";
        CabinetDOM cab = new CabinetDOM(filename);
        
        String xpathExpression = "//cab:infirmier";
        XmlNodeList nbInf = cab.getXpath("cab", "http://www.univ-grenoble-alpes.fr/l3miage/medical", xpathExpression);

        string inf = "infirmier";
        int nbInfirmier = cab.count(inf);
        Console.WriteLine("-> Nombre d' {0} : {1}", inf, nbInfirmier);

        string pat = "patient";
        int nbPatient = cab.count(pat);
        Console.WriteLine("-> Nombre de {0} : {1}", pat, nbPatient);
        
        bool res = cab.adresseCabinetEstComplete();
        Console.WriteLine("-> Adresse Cabinet complete ?? : {0}", res);

        String nomTestPatient = "Pien"; //Nom du patient dont on veut verifier l'adresse
        bool resPatient = cab.adressePatientEstComplete(nomTestPatient);
        Console.WriteLine("-> Adresse du patient '{0}' complète ?? : {1}", nomTestPatient, resPatient);
        
        
        // -------------- Modification de l'arbre avec DOM --------------
        //On ne peut pas ajouter un nouvel infirmier et un patient à la fois (soit l'un soit l'autre), une exception est levée sinon... 

        //-------- Ajout de l'infirmier Jean Némard (resultat dans newCabinetDOM.xml)  ---------
        //cab.addInfirmier("Némard", "Jean");
        Console.WriteLine("Nouveau infirmier ajouté dans newCabinetDOM.xml");

        //-------- Ajout du patient Burhan KIKS (resultat dans newCabinetDOM.xml)  ---------
        //Mis en commentaire car on ne peut ajouter un nouveau infirmier et un nouveau patient
        
        Adresse adresseDom = new Adresse(5, 6, "rue de la paix", "38100", "Chicagre");
        cab.addPatient("KIKS", "Burhan", "2000-03-04", "102039999988876", adresseDom);
        Console.WriteLine("Nouveau patient ajouté dans newCabinetDOM.xml");

        List<Acte> listActeId = new List<Acte>();
        listActeId.Add(new Acte("101"));
        listActeId.Add(new Acte("102"));
        listActeId.Add(new Acte("103"));
        cab.addVisite("2026-01-04", 003, listActeId, "KIKS");
        Console.WriteLine("Ajout de visite de KIKS dans newCabinetDOM.xml");
        
        
        // -------- methode nssValide(nomPatient) qui verifie que le numero de securite social de nomPatient est valide par rapport aux informations --------
        //renvoie un boolean
        bool resultNSS = cab.nssValide("BARKOK");
        
        
        //-------------- Partie serealisation -------------- 
        
        
        //-------- seralisation d'une adresse --------
        var adrManager = new XMLManager<Adresse>();
        Adresse TestAdresse = new Adresse(12, 1, "rue de la paix", "69000", "Lyon");
        //Les 2 lignes suivantes permet de sauvegarder dans une instance xml (adresse.xml), ceci nous permet de le tester
        //mais n'est pas necessaire pour serealiser le cabinet complet (ils ne seront pas presentes lors des serealisation qui suivent
        /*string pathAdr = "../../../data/xml/adresse.xml";
        adrManager.Save(pathAdr, TestAdresse);*/ 
        
        
        //-------- seralisation d'un infirmier --------
        var infirManager = new XMLManager<Infirmier>();
        var TestInfirmier = new Infirmier("005", "BARKOK0", "Omar", "omar.png");
        string pathInfir = "../../../data/perso/infirmier.xml";
        
        
        //-------- seralisation de infirmiers (liste d'infirmier) --------
        var InfirmiersManager = new XMLManager<Infirmiers>();
        List<Infirmier> listInfirmiers = new List<Infirmier>();
        listInfirmiers.Add(TestInfirmier);
        var infirmiersSer = new Infirmiers(listInfirmiers);
        
        
        //-------- seralisation d'un acte --------
        XMLManager<Acte> acteManager = new XMLManager<Acte>();
        Acte acteSerealisation = new Acte("503");
        
        
        //-------- seralisation d'une visite --------
        List<Acte> listActes = new List<Acte>();
        listActes.Add(acteSerealisation);
        XMLManager<Visite>  visiteManager = new XMLManager<Visite>();
        Visite visiteSereal = new Visite("005", "2025-01-23", listActes);
        
        
        //-------- seralisation d'un patient --------
        XMLManager<Patient>  patientManager = new XMLManager<Patient>();
        Patient patientSereal = new Patient("Orouge", "Elvire", "F", "1982-03-08", "282036912305243", TestAdresse, visiteSereal);
        
        
        //-------- seralisation de patients (liste de patient) --------
        XMLManager<Patients> patientsManager = new XMLManager<Patients>();
        List<Patient> patientsList = new List<Patient>();
        patientsList.Add(patientSereal);
        Patients patientsSeral = new Patients(patientsList);
        
        
        //-------- seralisation du cabinet (avec les infos dessus) --------
        XMLManager<Cabinet> cabinetManager = new XMLManager<Cabinet>();
        Cabinet cabientSereal = new  Cabinet("Mon Cabinet", TestAdresse, infirmiersSer, patientsSeral);
        cabinetManager.Save("../../../data/xml/cabinet_modif.xml",cabientSereal);
        
        
        //-------------- Partie deserealisation -------------- 
        //Partie 7.4.4 ajout d'un patient au cabinet
        
        XMLManager<Cabinet> cabinetDeserealManager = new XMLManager<Cabinet>();
        Cabinet cabinetDesereal = cabinetDeserealManager.Load("../../../data/xml/cabinet.xml"); //recuperation des données
        
        Acte newActe = new Acte("105"); //Ajout d'un nouvel acte
        List<Acte> newListActe = new List<Acte>();
        newListActe.Add(newActe);
        
        Visite newVisite = new Visite("001", "2026-01-01", newListActe); //ajout d'une nouvelle visite au patient
        Adresse newAdresse = new Adresse(3, 66, "route du 66", "75000", "Paris"); // ajout de l'adresse du patient
        Patient newPatient = new Patient("KULLS", "Kass", "M", "2005-01-08", "105011356797864", newAdresse, newVisite); //ajout d'un nouveau patient
        
        cabinetDesereal.Patients.addPatient(newPatient); //On ajoute ne nouveau patient à la fin de la liste des patients de notre cabinet
        
        //Sauvegarde du nouveau cabinet (RESULTAT DANS : cabinet_modif.xml)
        cabinetDeserealManager.Save("../../../data/xml/cabinet_modif.xml", cabinetDesereal); 
    }
}