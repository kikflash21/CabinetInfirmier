namespace CabinetInfirmier;

using System.Xml.Serialization;

public class XMLManager<T>
{
    public T Load(string path) {
        // Declare a generic T variable of the type to be deserialized to store the deserialized xml file 
        T _instance;
        // reads the text (xml) file and store in reader
        using (TextReader reader = new StreamReader(path)) {
            // create an instance of the XmlSerializer
            var xml = new XmlSerializer(typeof(T));
            // deserialize and casts the text into the type T
            _instance = (T) xml.Deserialize(reader);
        }

        // returns the document as a serialized object of type T
        return _instance;
    }
    
    public void Save(string path, object obj) {
        using (TextWriter writer = new StreamWriter(path)) {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj);
        }
    }
    
    public void Save(string path, object obj, XmlSerializerNamespaces ns) {
        using (TextWriter writer = new StreamWriter(path)) {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj, ns);
        }
    }
}