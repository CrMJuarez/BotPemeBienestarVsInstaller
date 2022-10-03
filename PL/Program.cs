using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PL
{
    public class Program
    {


        public static void Main(string[] args)
        {

            DatosPortal dp = new DatosPortal();
            dp.ExtraerDatos();
            //----->
            //comentar para poder depurar
            //Timer timer = new Timer(15000);//30000

            //timer.Elapsed += EventoElapsed;
            //timer.Start();

            //while (true) ;
            /////<-------

        }
        //----->
        //comentar para poder depurar

        //private static void EventoElapsed(object sender, ElapsedEventArgs e)
        //{
        //    DatosPortal dp = new DatosPortal();
        //    dp.ExtraerDatos();


        //}
        /////<-------

    }
}
