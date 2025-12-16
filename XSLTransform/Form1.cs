using System.Globalization;
using System.Xml.Linq;
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
            try
            {
                var xslt = new XslCompiledTransform();
                xslt.Load("transform.xsl");
                xslt.Transform("Data1.xml", "Employees.xml");

                AddTotalAttributeToEmployees();

                MessageBox.Show("Преобразование и обновление выполнены успешно!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AddTotalAttributeToEmployees()
        {
            if (!File.Exists("Employees.xml"))
                throw new FileNotFoundException("Файл Employees.xml не найден после XSLT-преобразования.");

            var doc = XDocument.Load("Employees.xml");
            var culture = CultureInfo.InvariantCulture;

            foreach (var employee in doc.Root.Elements("Employee"))
            {
                double total = 0.0;

                foreach (var salary in employee.Elements("salary"))
                {
                    var amountAttr = salary.Attribute("amount");
                    if (amountAttr?.Value != null)
                    {
                        string amountStr = amountAttr.Value.Replace(",", ".");
                        if (double.TryParse(amountStr, NumberStyles.Float, culture, out double value))
                        {
                            total += value;
                        }
                    }
                }
                employee.SetAttributeValue("total", total.ToString(culture));
            }

            doc.Save("Employees.xml");
        }
    }
}
