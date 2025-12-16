using System.Xml.Xsl;

namespace XSLTransform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load("transform.xsl");
            xslt.Transform("Data1.xml", "Employees.xml");
        }
    }
}
