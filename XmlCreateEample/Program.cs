using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using XmlCreateEample.Xmls;

namespace XmlCreateEample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //XmlGenerator.Execute();

            //AttributeRead.Execute();

            var username = new Username { Name = "Username", TextContent = "ABCDE" };
            var password = new Password { Name = "Password", TextContent = "123456" };
            var usernameToken = new UsernameToken
            {
                Name = nameof(UsernameToken),
                Children = new List<XmlElement> { username, password },
            };

            var security = new Security
            {
                Name = nameof(Security),
                Children = new List<XmlElement> { usernameToken },
            };
            var header = new Header
            {
                Name = nameof(Header),
                Children = new List<XmlElement> { security },
            };
            var root = new Envelope
            {
                Name = nameof(Envelope),
                Children = new List<XmlElement> { header }
            };

            var resultContent = new Xmls.XmlGenerator().GenerateToString(root);

            Thread thread = new Thread(() => Clipboard.SetData(DataFormats.Text, resultContent));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
        }
    }

    [XmlElementPrefix("env", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", SetNamespace = true)]
    [XmlElementAttr("xsd", "http://www.w3.org/2001/XMLSchema")]
    [XmlElementAttr("xsi", "http://www.w3.org/2001/XMLSchema-instance")]
    [XmlElementAttr("ns0", "http://xmlns.mrs.com.br/notification/header/")]
    [XmlElementAttr("ns1", "http://xmlns.mrs.com.br/iti/tipos/eventosFerro")]
    [XmlElementAttr("ns2", "http://xmlns.mrs.com.br/iti/tipos/evento")]
    [XmlElementAttr("ns3", "http://xmlns.mrs.com.br/iti/tipos/comuns")]
    [XmlElementAttr("ns4", "http://xmlns.mrs.com.br/iti/tipos/notaFiscal")]
    [XmlElementAttr("ns5", "http://xmlns.mrs.com.br/iti/tipos/despacho")]
    [XmlElementAttr("ns6", "http://xmlns.mrs.com.br/iti/tipos/retorno")]
    public class Envelope : XmlElement { }


    [XmlElementPrefix("env", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Header : XmlElement { }

    [XmlElementPrefix("wsse", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", SetNamespace = true)]
    [XmlElementAttr("mustUnderstand", "1", NamespacePrefix = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Security : XmlElement { }

    [XmlElementPrefix("wsse", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class UsernameToken : XmlElement { }

    [XmlElementPrefix("wsse", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class Username : XmlElement { }

    [XmlElementPrefix("wsse", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    [XmlElementAttr("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText")]
    public class Password : XmlElement { }
}
