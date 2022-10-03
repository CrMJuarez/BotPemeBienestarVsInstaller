using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using System.Timers;

namespace RespuestaBotService1
{
    public partial class BotRespuestaService1 : ServiceBase
    {
        Timer Timer = new Timer();
        int Interval = 10000; // 10000 ms = 10 second 3000= 3 second
        
        public BotRespuestaService1()
        {
            InitializeComponent();
            eventoSistema = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("BotRespuestaService1"))
            {
                System.Diagnostics.EventLog.CreateEventSource("BotRespuestaService1", "Application");
            }
            eventoSistema.Source = "BotRespuestaService1";
            eventoSistema.Log = "Application";
        }
        protected override void OnStart(string[] args)
        {
            Timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            Timer.Interval = Interval;
            Timer.Enabled = true;
            ServiceController servicio = new ServiceController("BotRespuestaService1");
            int timeoutMilisegundos = 10000;//10000 con este tiempo funciona 
            try                             //300000 = 5 min       
            {
                PL.DatosPortal datosPortal = new PL.DatosPortal();
                datosPortal.ExtraerDatos();
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
                servicio.WaitForStatus(ServiceControllerStatus.Running, timeout);         
                eventoSistema.WriteEntry("Se ha iniciado el servicio de respuesta (BotRespuestaService1).");
                //ServiceController servicio = new ServiceController("BotRespuestaService1");
                //servicio.Stop();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "Excepción: " + ex.Message);
            }
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {

                //WriteLog("{0} ms elapsed.");
                PL.DatosPortal datosPortal = new PL.DatosPortal();
                datosPortal.ExtraerDatos();

                //si no funciona hay que comentar estas dos lineas 
                //ServiceController servicio = new ServiceController("BotRespuestaService1");
                //servicio.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "Excepción: " + ex.Message);
            }
        }
        protected override void OnStop()
        {
            eventoSistema.WriteEntry("Se ha detenido el servicio de respuesta (BotRespuestaService1).");
            
        }
        //private void WriteLog(string logMessage, bool addTimeStamp = true)
        //{
        //    var path = AppDomain.CurrentDomain.BaseDirectory;
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);

        //    var filePath = String.Format("{0}\\{1}_{2}.txt",
        //        path,
        //        ServiceName,
        //        DateTime.Now.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
        //        );

        //    if (addTimeStamp)
        //        logMessage = String.Format("[{0}] - {1}",
        //            DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture),
        //            logMessage);

        //    File.AppendAllText(filePath, logMessage);            
        //}
       
    }
    
}
