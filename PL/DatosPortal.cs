using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
namespace PL

{

    public class DatosPortal
    {
        public string Pagesrc { get; set; }

        public void ExtraerDatos()

        {
            //----------->
            //Se colocan los chrome options para evitar que se abra el navegador en segundo plano y todo lo haga internamente
            //COMENTAR si se van a hacer pruebas 
            //var timeOutTime = TimeSpan.FromMinutes(4);
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArguments(new List<string>() {
            "--silent-launch",
            "--no-startup-window",
            "no-sandbox",
            "headless",});

            //<----------------------

            //instancia del navegador en segundo plano
            IWebDriver driver = new ChromeDriver(chromeOptions);/*chromeOptions/*///<---Quitar valor chromeOptions si se van a hacer pruebas


            string Url = System.Configuration.ConfigurationManager.AppSettings["Page"].ToString();
            driver.Navigate().GoToUrl(Url);
            //se mandan las credenciales 

            string User = System.Configuration.ConfigurationManager.AppSettings["txUsuario"].ToString();
            string Password = System.Configuration.ConfigurationManager.AppSettings["txtPassword"].ToString();

            var Input = driver.FindElement(By.Id("txtUsuario"));
            Input.SendKeys(User);

            var Input1 = driver.FindElement(By.Id("txtPassword"));
            Input1.SendKeys(Password);
            //se hace input al boton de login
            var Input2 = driver.FindElement(By.Name("imgLogin"));
            Input2.Submit();


            //se pone un tiempo de espera para que cargue los elementos
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            //se hace uso de switchto para esoger el frame por nombre
            driver.SwitchTo().Frame("fraEncabezado");
            //se pone ActiveElement(); para que mantenga cargado lo que esta dentro del frame y asi nos muestre el contenido en html
            driver.SwitchTo().ActiveElement();

            //se selecciona el combo con la opcion Monitor de Solicitudes TV para pasar a la siguente pagina con el evento click
            driver.FindElement(By.XPath("//select[@id='" + "cboMenus" + "']/option[contains(.,'" + "Monitor de Solicitudes TV" + "')]")).Click();

            //A diferencia del active aqui si necesitamos el contenido por default para que podamos encontrar ahoara el frame Principal
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("fraPrincipal");
            //una vez que encontro el fraPrincipal ahora si que nos traiga el contenido activo dentro de el
            driver.SwitchTo().ActiveElement();

            //seleccionamos el boton para que cargue lso datos en la tabla con un evento click
            driver.FindElement(By.Id("btnFilFecha")).Click();
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //Cierra las ventanas creadas una vez que ya logramos obtener el html 

            //guardamos el contenido del html generado en una variable tipo string

            Pagesrc = driver.PageSource;

            //Cierra las ventanas creadas una vez que ya logramos obtener el html 
            //Libera todos los recursos 
            driver.Quit();
            SepararDatos();
        }


        public void SepararDatos()

        {

            //declaramos una variable tippo doc que toma por valor htmldocument
            var doc = new HtmlDocument();

            //se carga el contenido de la variable pagesrc y lo convierte a htmldocument
            doc.LoadHtml(Pagesrc);

            //se encarga de encontrar dentro del documento la tabla que nos interesa 
            var myTable = doc.DocumentNode
                 .Descendants("div")
                 .Where(t => t.Attributes["id"].Value == "tablaJson")
                 .FirstOrDefault();

            //se hace un foreach para que selecciones los datos de la tabla
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table[1]"))
            {
                //segundo foreach para que divida el documento por div
                foreach (HtmlNode row in table.SelectNodes("//div"))
                {
                    //  tercer foreach para dividir el documento pot tr
                    foreach (HtmlNode cell in row.SelectNodes("//tr"))
                    {
                        //trae los td de forma decendiente y en un arreglo
                        var tds = cell.Descendants("td").ToArray(); // trae todos los td

                        //condicion para que cuando detecte los headers que esos datos no nos sirven entre al if
                        //y si el valor es igual al primero del arreglo de tds continue con el 
                        //siguiente ciclo que es en el que ya vienen los datos importantes
                        if (tds.Count() == 19)
                        {
                            var Valor = "Id";

                            if (tds[0].InnerHtml.Equals(Valor))
                            {
                                continue;
                            }
                        }
                        //condicion para que en cuanto detecte que hay 19 nodos que son los que pertenecen a la tabla obtenga esos datos

                        if (tds.Count() == 19)
                        {
                            //guarda los datos en las variables correspondientes para agregar a la base de datos

                            ML.DatosPortal datosPortal = new ML.DatosPortal();

                            //List<object> tss = new List<object>().ToList();
                            datosPortal.Prioridad = tds[1].InnerText.ToString();
                            datosPortal.TipoServicio = tds[2].InnerText.ToString();
                            datosPortal.SucursalConsignatario = tds[3].InnerText.ToString();
                            datosPortal.FechaCaptura = tds[4].InnerText.ToString();
                            datosPortal.FechaRealizarServicio = tds[5].InnerText.ToString();
                            datosPortal.IdFolioDeServicio = tds[6].InnerText.ToString();
                            datosPortal.OrdenServicio = tds[7].InnerText.ToString();
                            char[] chars = { ' ' };
                            string Imp = tds[8].InnerText;
                            string Import = Imp.Trim(chars);
                            datosPortal.Importe = decimal.Parse(Import.ToString());
                            datosPortal.Divisa = tds[9].InnerText.ToString();
                            datosPortal.Te = tds[10].InnerText.ToString();
                            datosPortal.HoraEnvio = tds[11].InnerText.ToString();
                            datosPortal.Actualizacion = tds[12].InnerText.ToString();
                            datosPortal.Estatus = tds[13].InnerText.ToString();

                            //intento con equals
                            if (datosPortal.IdFolioDeServicio == null)
                            {
                                Console.WriteLine("No existe formato valido de folio de servicio");
                            }
                            else
                            {
                                ML.Result result = BL.DatosPortal.GetById(datosPortal.IdFolioDeServicio);
                                if (result.Correct)
                                {
                                    if (datosPortal == result.Object)
                                    {
                                        Console.WriteLine("No hay cambios en ningun campo");
                                    }
                                    else
                                    {
                                        BL.DatosPortal.Update(datosPortal);
                                        Console.WriteLine("Se modificaron los datos");

                                    }
                                }
                                else
                                {

                                    BL.DatosPortal.Add(datosPortal);
                                    Console.WriteLine("Se registraron los datos");

                                }
                            }
                        }
                    }
                    //se pone break para que salga del ciclo despues de insertan todos los datos ya que si no se pone no para nunca
                    break;
                }
                //se pone break para que salga del ciclo principal
                break;
            }
        }
    }
}