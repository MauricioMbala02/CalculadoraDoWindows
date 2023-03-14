using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //
        // Declarar Variáveis
        //

        bool vaiMudar = false, fechaParenteses = false, fe = false, hyp = false;
        string deg = "deg";
        Panel pnHis, pnMem;
        Label lbl1His, lbl2His, lbl1Mem;
        int nHis = 1, nMem = 1;
        Label[] lb1His = new Label[0], lb2His = new Label[0], lb1Mem = new Label[0];

        //
        // Deixar Responsivo
        //

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width < 800)
            {
                splitTela.Panel2Collapsed = true;
            }
            else
            {
                splitTela.Panel2Collapsed = false;
            }
        }

        //
        // Faz a Conta
        //

        private double Evaluate(string expression)
        {
            expression = expression.Replace(",", ".");
            expression = expression.Replace("÷", "/");
            expression = expression.Replace("×", "*");
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), expression);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }
            catch
            {
                return double.Parse("0");
            }
        }

        //
        // Decimal para Graus
        //

        private string DecimalToDegree(string dec)
        {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }

        //
        // Decimal para Minutos
        //

        private string DecimalToMinutes(string dec)
        {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) * 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }

        //
        // Criar o Histórico
        //

        private void Historico(string num, string men)
        {
            pnHis = new Panel();
            lbl1His = new Label();
            lbl2His = new Label();

            pnHis.Height = 70;
            pnHis.Dock = DockStyle.Top;
            pnHis.MouseEnter += new EventHandler(PnFoco);
            pnHis.MouseLeave += new EventHandler(PnForaFoco);
            pnHis.Click += new EventHandler(PnCliqueHis);
            pnHis.Name = "pn" + nHis;


            lbl1His.Text = men + " =";
            lbl1His.Font = new Font("Segoe UI", 10);
            lbl1His.Name = "lb1_" + nHis;
            lbl1His.Left = panelHistorico.Width- lbl1His.Width;

            lbl2His.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl2His.Text = num;
            lbl2His.Top = 20;
            lbl2His.Name = "lb2_" + nHis;
            lbl2His.Height = 32;
            lbl2His.Left = panelHistorico.Width - lbl2His.Width;

            panelHistorico.Controls.Add(pnHis);
            pnHis.Controls.Add(lbl2His);
            pnHis.Controls.Add(lbl1His);

            Array.Resize(ref lb1His, nHis);
            Array.Resize(ref lb2His, nHis);
            lb1His[nHis - 1] = lbl1His;
            lb2His[nHis - 1] = lbl2His;
            nHis++;
        }

        private void PnFoco(object sender, EventArgs e)
        {
            Panel pn = sender as Panel;
            pn.BackColor = Color.Silver;
        }

        private void PnForaFoco(object sender, EventArgs e)
        {
            Panel pn = sender as Panel;
            pn.BackColor = Color.Transparent;
        }

        private void PnCliqueHis(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            lbl1His = lb1His[int.Parse(a) - 1];
            lbl2His = lb2His[int.Parse(a) - 1];

            lblResultado.Text = lbl2His.Text;
            lblConta.Text = lbl1His.Text.Remove(lbl1His.Text.Length - 2, 2);

            vaiMudar = false;
            fechaParenteses = false;
        }

        //
        // Cria a Memória
        //
        private void Memoria(string num)
        {
            pnMem = new Panel();
            lbl1Mem = new Label();

            pnMem.Height = 70;
            pnMem.Dock = DockStyle.Top;
            pnMem.MouseEnter += new EventHandler(PnFoco);
            pnMem.MouseLeave += new EventHandler(PnForaFoco);
            pnMem.Click += new EventHandler(PnCliqueMem);
            pnMem.Name = "pn" + nMem;

            lbl1Mem.Text = num;
            lbl1Mem.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl1Mem.Name = "lb1_" + nMem;
            lbl1Mem.Left = panelMemoria.Width - lbl1Mem.Width;
            lbl1Mem.Height = 35;

            panelMemoria.Controls.Add(pnMem);
            pnMem.Controls.Add(lbl1Mem);

            Array.Resize(ref lb1Mem, nMem);
            lb1Mem[nMem - 1] = lbl1Mem;
            nMem++;
        }

        private void PnCliqueMem(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            lbl1Mem = lb1Mem[int.Parse(a) - 1];

            lblResultado.Text = lbl1Mem.Text;

            vaiMudar = false;
            fechaParenteses = false;
        }
        //
        // Botões da Calculadora PRINCIPAIS
        //

        private void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if(lblResultado.Text == "0" || vaiMudar)
            {
                lblResultado.Text = "";
                vaiMudar = false;
            }
            lblResultado.Text += btn.Text;
        }

        private void btnOpe_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string aux;
            if (fe)
                aux = double.Parse(lblResultado.Text).ToString("0.###E+0");
            else
                aux = lblResultado.Text;
            if (fe)
                lblResultado.Text = Evaluate(lblConta.Text + lblResultado.Text).ToString("0.###E+0");
            else
                lblResultado.Text = Evaluate(lblConta.Text + lblResultado.Text).ToString();
            if(fechaParenteses)
                lblConta.Text += " " + btn.Text + " ";
            else
                lblConta.Text += aux + " " + btn.Text + " ";
            vaiMudar = true;
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "0";
        }

        private void ctnC_Click(object sender, EventArgs e)
        {
            lblConta.Text = "";
            lblResultado.Text = "0";
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            lblResultado.Text = lblResultado.Text.Substring(0, lblResultado.Text.Length-1);
        }

        private void btnVirgula_Click(object sender, EventArgs e)
        {
            if(!lblResultado.Text.Contains(','))
            {
                lblResultado.Text += ",";
            }
        }

        private void btnMaisouMenos_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (double.Parse(lblResultado.Text) * -1).ToString();
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            string conta = lblConta.Text + lblResultado.Text;
            double resultado = Evaluate(conta);
            if (fe)
                lblResultado.Text = resultado.ToString("0.###E+0");
            else
                lblResultado.Text = resultado.ToString();
            Historico(resultado.ToString(), conta);
            lblConta.Text = "";
            vaiMudar = true;
        }

        //
        // Botões da Calculadora Funções 1 linha
        //

        private void btnXquad_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), 2).ToString();
            vaiMudar = true;
        }

        private void btnXeleY_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Evaluate(lblConta.Text + lblResultado.Text).ToString();
            lblConta.Text += aux + " ^ ";
            vaiMudar = true;
        }

        private void btnSin_Click(object sender, EventArgs e)
        {
            if(hyp)
                lblResultado.Text = Math.Sinh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Sin(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Cosh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Cos(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnTan_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Tanh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Tan(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        //
        // Botões da Calculadora Funções 2 linha
        //

        private void btnXcubo_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), 3).ToString();
            vaiMudar = true;
        }

        private void btnYraizX_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Evaluate(lblConta.Text + lblResultado.Text).ToString();
            lblConta.Text += aux + " yroot ";
            vaiMudar = true;
        }

        private void btnSin1_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Sinh(double.Parse(lblResultado.Text)),-1).ToString();
            else
                lblResultado.Text = Math.Asin(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnCos1_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Cosh(double.Parse(lblResultado.Text)), -1).ToString();
            else
                lblResultado.Text = Math.Acos(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnTan1_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Tanh(double.Parse(lblResultado.Text)), -1).ToString();
            else
                lblResultado.Text = Math.Atan(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        //
        // Botões da Calculadora Funções 3 linha
        //

        private void btnRaiz_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Sqrt(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnDezaX_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(10, double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Log(double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            string resultado = lblResultado.Text;
            if(resultado.Contains(','))
                lblResultado.Text = resultado + "E+";
            else
                lblResultado.Text = resultado + ",E+";
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Evaluate(lblConta.Text + lblResultado.Text).ToString();
            lblConta.Text += aux + " Mod ";
            vaiMudar = true;
        }

        //
        // Botões da Calculadora Funções 4 linha
        //

        private void btnUmSobreX_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (1/double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnEaX_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(Math.E, double.Parse(lblResultado.Text)).ToString();
            vaiMudar = true;
        }

        private void btnLn_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (Math.Log(double.Parse(lblResultado.Text))/Math.Log(Math.E)).ToString();
            vaiMudar = true;
        }

        private void btnDms_Click(object sender, EventArgs e)
        {
            lblResultado.Text = DecimalToMinutes(lblResultado.Text);
            vaiMudar = true;
        }

        private void btnMc_Click(object sender, EventArgs e)
        {
            try
            {
                lb1Mem = new Label[0];
                panelMemoria.Controls.Clear();
            }
            catch
            {

            }
        }

        private void btnMr_Click(object sender, EventArgs e)
        {
            try
            {
                string a = pnMem.Name.Remove(0, 2);
                lbl1Mem = lb1Mem[int.Parse(a) - 1];

                lblResultado.Text = lbl1Mem.Text;

                vaiMudar = false;
                fechaParenteses = false;
            }
            catch
            {

            }
        }

        private void btnMMais_Click(object sender, EventArgs e)
        {
            double soma = double.Parse(lblResultado.Text);
            double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
            double resultado = soma + a;
            panelMemoria.Controls.Remove(pnMem);
            Memoria(resultado.ToString());
            vaiMudar = true;
            fechaParenteses = false;
        }

        private void btnMMenos_Click(object sender, EventArgs e)
        {
            double soma = double.Parse(lblResultado.Text);
            double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
            double resultado = a - soma;
            panelMemoria.Controls.Remove(pnMem);
            Memoria(resultado.ToString());
            vaiMudar = true;
            fechaParenteses = false;
        }

        private void btnDeg_Click(object sender, EventArgs e)
        {
            lblResultado.Text = DecimalToDegree(lblResultado.Text);
            vaiMudar = true;
        }

        private void btnMs_Click(object sender, EventArgs e)
        {
            Memoria(lblResultado.Text);
        }

        //
        // Botões da Calculadora Funções 5 linha
        //

        private void btnPi_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.PI.ToString();
            vaiMudar = true;
        }

        private void btnFatorial_Click(object sender, EventArgs e)
        {
            double num = double.Parse(lblResultado.Text);
            double fatorial = 1;
            for(double i = num; i>0; i--)
            {
                fatorial *= i;
            }
            lblResultado.Text = fatorial.ToString();
            vaiMudar = true;
        }

        private void btnAbrePar_Click(object sender, EventArgs e)
        {
            lblConta.Text += " ( ";
            vaiMudar = true;
        }

        private void btnFechaPar_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblConta.Text += aux + " ) ";
            vaiMudar = true;
            fechaParenteses = true;
        }


        //
        // Funções linha de cima
        //

        private void btnHyp_Click(object sender, EventArgs e)
        {
            if (hyp)
            {
                hyp = false;
                btnHyp.FlatAppearance.BorderSize = 0;
            }
            else
            {
                hyp = true;
                btnHyp.FlatAppearance.BorderColor = Color.Red;
                btnHyp.FlatAppearance.BorderSize = 3;
            }
        }

        private void btnFe_Click(object sender, EventArgs e)
        {
            if(fe)
            {
                fe = false;
                btnFe.FlatAppearance.BorderSize = 0;
                lblResultado.Text = double.Parse(lblResultado.Text).ToString();
            }
            else
            {
                fe = true;
                btnFe.FlatAppearance.BorderColor = Color.Red;
                btnFe.FlatAppearance.BorderSize = 3;
                lblResultado.Text = double.Parse(lblResultado.Text).ToString("0.###E+0");
            }
        }

        private void btnDeg1_Click(object sender, EventArgs e)
        {
            if(deg == "deg")
            {
                btnDeg1.Text = "RAD";
                deg = "rad";
            }
            else if(deg == "rad")
            {
                btnDeg1.Text = "GRAD";
                deg = "grad";
            }
            else
            {
                btnDeg1.Text = "DEG";
                deg = "deg";
            }
        }

    }
}
