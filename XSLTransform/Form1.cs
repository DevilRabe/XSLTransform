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

        private List<Employee> _employees = new List<Employee>();
        private Dictionary<string, double> _totalMonthly = new Dictionary<string, double>();

        private class Employee
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Total { get; set; }
            public List<SalaryRecord> Salaries { get; set; } = new List<SalaryRecord>();
        }

        private class SalaryRecord
        {
            public string Month { get; set; }
            public double Amount { get; set; }
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

                LoadDataFromEmployeesXml();
                //LoadEmployeesIntoGrid();
                //LoadMonthlyTotalsIntoGrid();


                MessageBox.Show("Преобразование и обновление выполнены успешно!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Добавить новую выплату";
                form.Width = 320;
                form.Height = 240;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;

                var layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 5,
                    Padding = new Padding(10),
                    AutoSize = true
                };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
                for (int i = 0; i < 5; i++)
                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var txtName = new TextBox { Dock = DockStyle.Fill };
                var txtSurname = new TextBox { Dock = DockStyle.Fill };
                var txtAmount = new TextBox { Dock = DockStyle.Fill };

                var cmbMonth = new ComboBox { Dock = DockStyle.Fill };

                string[] months = { "january", "february", "march", "april", "may", "june",
                            "july", "august", "september", "october", "november", "december" };
                cmbMonth.Items.AddRange(months);
                cmbMonth.SelectedIndex = 0;

                layout.Controls.Add(new Label { Text = "Имя:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
                layout.Controls.Add(txtName, 1, 0);

                layout.Controls.Add(new Label { Text = "Фамилия:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
                layout.Controls.Add(txtSurname, 1, 1);

                layout.Controls.Add(new Label { Text = "Сумма:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
                layout.Controls.Add(txtAmount, 1, 2);

                layout.Controls.Add(new Label { Text = "Месяц:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 3);
                layout.Controls.Add(cmbMonth, 1, 3);

                var btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, 10, 0, 0)
                };

                var btnCancel = new Button { Text = "Отмена", Width = 80, DialogResult = DialogResult.Cancel };
                var btnAdd = new Button { Text = "Добавить", Width = 80 };
                btnAdd.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text) ||
                        string.IsNullOrWhiteSpace(txtSurname.Text) ||
                        string.IsNullOrWhiteSpace(txtAmount.Text))
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string amountStr = txtAmount.Text.Replace(",", ".");
                    if (!double.TryParse(amountStr, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                    {
                        MessageBox.Show("Некорректная сумма!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string selectedMonth = cmbMonth.SelectedItem?.ToString() ?? "january";

                    AppendNewItemToData1(
                        txtName.Text.Trim(),
                        txtSurname.Text.Trim(),
                        txtAmount.Text.Trim(),
                        selectedMonth
                    );
                    PerformFullRecalculation();

                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                btnPanel.Controls.Add(btnAdd);
                btnPanel.Controls.Add(btnCancel);

                layout.Controls.Add(btnPanel, 1, 4);
                form.Controls.Add(layout);

                txtAmount.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                        btnAdd.PerformClick();
                };

                form.ShowDialog(this);
            }
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            dgvMonthly.Rows.Clear();
            dgvMonthly.Columns.Clear();
            dgvMonthly.Columns.Add("Month", "Месяц");
            dgvMonthly.Columns.Add("Amount", "Сумма");

            var culture = CultureInfo.InvariantCulture;

            if (dgvEmployees.SelectedRows.Count == 0)
            {
                foreach (var kvp in _totalMonthly.OrderBy(x => x.Key))
                {
                    dgvMonthly.Rows.Add(kvp.Key, kvp.Value.ToString("N2", culture));
                }
            }
            else
            {
                var selectedRow = dgvEmployees.SelectedRows[0];
                string name = selectedRow.Cells["Name"].Value?.ToString() ?? "";
                string surname = selectedRow.Cells["Surname"].Value?.ToString() ?? "";

                var emp = _employees.FirstOrDefault(e => e.Name == name && e.Surname == surname);
                if (emp != null)
                {
                    foreach (var sal in emp.Salaries)
                    {
                        dgvMonthly.Rows.Add(sal.Month, sal.Amount.ToString("N2", culture));
                    }
                }
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
                throw new FileNotFoundException("Файл Employees.xml не найден!");

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

        private void AppendNewItemToData1(string name, string surname, string amount, string month)
        {
            if (!File.Exists("Data1.xml"))
                throw new FileNotFoundException("Файл Data1.xml не найден!");

            var doc = XDocument.Load("Data1.xml");
            var newItem = new XElement("item",
                new XAttribute("name", name),
                new XAttribute("surname", surname),
                new XAttribute("amount", amount),
                new XAttribute("mount", month)
            );

            doc.Root?.Add(newItem);
            doc.Save("Data1.xml");
        }
        private void PerformFullRecalculation()
        {
            try
            {
                var xslt = new XslCompiledTransform();
                xslt.Load("transform.xsl");
                xslt.Transform("Data1.xml", "Employees.xml");

                AddTotalAttributeToEmployees();
                AddTotalToPayInData1();
                LoadDataFromEmployeesXml();

                MessageBox.Show("Данные обновлены!", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пересчёте: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadDataFromEmployeesXml()
        {
            _employees.Clear();

            if (!File.Exists("Employees.xml"))
                throw new FileNotFoundException("Файл Employees.xml не найден!");

            var doc = XDocument.Load("Employees.xml");
            var culture = CultureInfo.InvariantCulture;

            foreach (var empElem in doc.Root.Elements("Employee"))
            {
                string name = empElem.Attribute("name")?.Value ?? "";
                string surname = empElem.Attribute("surname")?.Value ?? "";
                string totalStr = empElem.Attribute("total")?.Value ?? "0";
                double total = double.TryParse(totalStr.Replace(",", "."), NumberStyles.Float, culture, out var t) ? t : 0;

                var emp = new Employee
                {
                    Name = name,
                    Surname = surname,
                    Total = total
                };

                foreach (var sal in empElem.Elements("salary"))
                {
                    string month = sal.Attribute("mount")?.Value ?? "unknown";
                    string amountStr = sal.Attribute("amount")?.Value ?? "0";
                    amountStr = amountStr.Replace(",", ".");
                    double amount = double.TryParse(amountStr, NumberStyles.Float, culture, out var a) ? a : 0;

                    emp.Salaries.Add(new SalaryRecord { Month = month, Amount = amount });
                }

                _employees.Add(emp);
            }

            foreach (var emp in _employees)
            {
                foreach (var sal in emp.Salaries)
                {
                    if (!_totalMonthly.ContainsKey(sal.Month))
                        _totalMonthly[sal.Month] = 0;
                    _totalMonthly[sal.Month] += sal.Amount;
                }
            }
            RefreshEmployeesGrid();
        }
        private void RefreshEmployeesGrid()
        {
            dgvEmployees.Rows.Clear();
            dgvEmployees.Columns.Clear();
            dgvEmployees.Columns.Add("Name", "Имя");
            dgvEmployees.Columns.Add("Surname", "Фамилия");
            dgvEmployees.Columns.Add("Total", "Общая сумма");

            foreach (var emp in _employees)
            {
                dgvEmployees.Rows.Add(emp.Name, emp.Surname, emp.Total.ToString("N2", CultureInfo.InvariantCulture));
            }

            dgvMonthly.Rows.Clear();
        }
    }
}
