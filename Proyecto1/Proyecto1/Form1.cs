using System.Data;
using System.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        string operacion = "";           
        double valor1 = 0;               
        bool operacionPresionada = false;
        string connectionString = "Server=ARTURO\\SQLEXPRESS;Database=Proyecto1_DS4;Trusted_Connection=True;"; 

        public Form1()
        {
            InitializeComponent();

            btnNum1.Click += new EventHandler(BotonNumero_Click);
            btnNum2.Click += new EventHandler(BotonNumero_Click);
            btnNum3.Click += new EventHandler(BotonNumero_Click);
            btnNum4.Click += new EventHandler(BotonNumero_Click);
            btnNum5.Click += new EventHandler(BotonNumero_Click);
            btnNum6.Click += new EventHandler(BotonNumero_Click);
            btnNum7.Click += new EventHandler(BotonNumero_Click);
            btnNum8.Click += new EventHandler(BotonNumero_Click);
            btnNum9.Click += new EventHandler(BotonNumero_Click);
            btnNum0.Click += new EventHandler(BotonNumero_Click);

            btnSum.Click += new EventHandler(BotonOperacion_Click);
            btnRes.Click += new EventHandler(BotonOperacion_Click);
            btnMul.Click += new EventHandler(BotonOperacion_Click);
            btnDiv.Click += new EventHandler(BotonOperacion_Click);
            btnRaiz.Click += new EventHandler(BotonOperacion_Click);
            btnPotencia.Click += new EventHandler(BotonOperacion_Click);

            btnResultado.Click += new EventHandler(botonResultado_Click);

            btnPunto.Click += new EventHandler(BotonPunto_Click);

        }

        private void BotonNumero_Click(object? sender, EventArgs e)
        {
            if (sender is Button boton)
            {
                if (txtDisplay.Text == "0" || operacionPresionada)
                {
                    txtDisplay.Clear(); 
                }

                operacionPresionada = false;
                txtDisplay.Text += boton.Text;  
            }
        }

        private void BotonOperacion_Click(object? sender, EventArgs e)
        {
            if (sender is Button boton)
            {
                operacion = boton.Text; 
                valor1 = Double.Parse(txtDisplay.Text); 
                operacionPresionada = true;
            }
        }

        private void botonResultado_Click(object? sender, EventArgs e)
        {
            try
            {
                decimal numero1 = Convert.ToDecimal(valor1);
                decimal numero2 = Convert.ToDecimal(txtDisplay.Text);
                string operacion = this.operacion;
                decimal resultado;

                switch (operacion)
                {
                    case "+":
                        resultado = numero1 + numero2;
                        break;
                    case "-":
                        resultado = numero1 - numero2;
                        break;
                    case "x":
                        resultado = numero1 * numero2;
                        break;
                    case "/":
                        resultado = numero1 / numero2;
                        break;
                    case "√":  
                        resultado = (decimal)Math.Sqrt((double)numero1);
                        break;
                    case "^": 
                        resultado = (decimal)Math.Pow((double)numero1, (double)numero2);
                        break;
                    default:
                        resultado = 0;
                        break;
                }

            
                txtDisplay.Text = resultado.ToString();

                GuardarCalculoEnBaseDeDatos(numero1, operacion, numero2, resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el cálculo: " + ex.Message);
            }
        }

        private void BotonPunto_Click(object? sender, EventArgs e)
        {
            if (sender is Button boton)
            {
                if (!txtDisplay.Text.Contains('.'))
                {
                    txtDisplay.Text += "."; 
                }
            }
        }
        private void GuardarCalculoEnBaseDeDatos(decimal numero1, string operacion, decimal numero2, decimal resultado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO CalculadoraCientifica " + "(Numero1, Operacion, Numero2, Resultado, FechaCalculo) VALUES (@numero1, @operacion, @numero2, @resultado, @fecha)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@numero1", numero1);
                    command.Parameters.AddWithValue("@operacion", operacion);
                    command.Parameters.AddWithValue("@numero2", numero2);
                    command.Parameters.AddWithValue("@resultado", resultado);
                    command.Parameters.AddWithValue("@fecha", DateTime.Now);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Guardado en la Base de Datos Proyecto1_DS4");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar cálculo: " + ex.Message);
                }
            }
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            valor1 = 0;
        }

        private void btnNumNegativo_Click(object sender, EventArgs e)
        {
            double numero = Double.Parse(txtDisplay.Text);
            numero = numero * -1;
            txtDisplay.Text = numero.ToString();
        }
    }
}
