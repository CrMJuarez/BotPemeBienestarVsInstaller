namespace RespuestaBotService1
{
    partial class BotRespuestaService1
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.eventoSistema = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventoSistema)).BeginInit();
            // 
            // eventoSistema
            // 
            this.eventoSistema.Log = "Application";
            // 
            // BotRespuestaService1
            // 
            this.ServiceName = "BotRespuestaService1";
            ((System.ComponentModel.ISupportInitialize)(this.eventoSistema)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog eventoSistema;
    }
}
