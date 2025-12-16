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

                AddTotalToPayInData1();

                LoadEmployeesIntoGrid();
                LoadMonthlyTotalsIntoGrid();


                MessageBox.Show("Преобразование и обновление выполнены успешно!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadEmployeesIntoGrid()
        {
            dgvEmployees.Rows.Clear();
            dgvEmployees.Columns.Clear();
            dgvEmployees.Columns.Add("Name", "Имя");
            dgvEmployees.Columns.Add("Surname", "Фамилия");
            dgvEmployees.Columns.Add("Total", "Общая сумма");

            if (!File.Exists("Employees.xml")) return;

            var doc = XDocument.Load("Employees.xml");
            var culture = CultureInfo.InvariantCulture;

            foreach (var emp in doc.Root.Elements("Employee"))
            {
                string name = emp.Attribute("name")?.Value ?? "";
                string surname = emp.Attribute("surname")?.Value ?? "";
                string totalStr = emp.Attribute("total")?.Value ?? "0";

                if (!double.TryParse(totalStr.Replace(",", "."), NumberStyles.Float, culture, out double total))
                    total = 0;

                dgvEmployees.Rows.Add(name, surname, total.ToString("N2", culture));
            }
        }

        private void LoadMonthlyTotalsIntoGrid()
        {
            dgvMonthly.Rows.Clear();
            dgvMonthly.Columns.Clear();
            dgvMonthly.Columns.Add("Month", "Месяц");
            dgvMonthly.Columns.Add("Amount", "Сумма");

            if (!File.Exists("Employees.xml"))
                throw new FileNotFoundException("Файл Employees.xml не найден после XSLT-преобразования.");

            var doc = XDocument.Load("Employees.xml");
            var culture = CultureInfo.InvariantCulture;

            var monthly = new Dictionary<string, double>();

            foreach (var salary in doc.Descendants("salary"))
            {
                string month = salary.Attribute("mount")?.Value ?? "unknown";
                string amountStr = salary.Attribute("amount")?.Value ?? "0";
                amountStr = amountStr.Replace(",", ".");

                if (double.TryParse(amountStr, NumberStyles.Float, culture, out double amount))
                {
                    if (!monthly.ContainsKey(month))
                        monthly[month] = 0;
                    monthly[month] += amount;
                }
            }

            foreach (var kvp in monthly.OrderBy(m => m.Key))
            {
                dgvMonthly.Rows.Add(kvp.Key, kvp.Value.ToString("N2", culture));
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

        private void AddTotalToPayInData1()
        {
            if (!File.Exists("Data1.xml"))
                throw new FileNotFoundException("Файл Data1.xml не найден.");

            var doc = XDocument.Load("Data1.xml");
            var culture = CultureInfo.InvariantCulture;
            double total = 0.0;

            foreach (var item in doc.Descendants("item"))
            {
                var amountAttr = item.Attribute("amount");
                if (amountAttr?.Value != null)
                {
                    string amountStr = amountAttr.Value.Replace(",", ".");
                    if (double.TryParse(amountStr, NumberStyles.Float, culture, out double value))
                    {
                        total += value;
                    }
                }
            }
            doc.Root?.SetAttributeValue("total", total.ToString(culture));

            doc.Save("Data1.xml");
        }
    }
}
