using System;
using System.Globalization;
using System.Timers;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Bienvenida
{
    /// <summary>
    /// Componente code-behind de la vista de Bienvenida.
    /// Despliega el saludo inicial tras la autenticación y mantiene un reloj en tiempo real.
    /// </summary>
    public partial class Bienvenida : ComponentBase, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CultureInfo _peCulture = new CultureInfo("es-PE");

        // Representación formateada en tiempo real de la fecha y hora local peruana
        public string CurrentDateTimeString { get; set; }

        /// <summary>
        /// Inicializa el componente, calcula la fecha inicial y arranca un temporizador en segundo plano.
        /// </summary>
        protected override void OnInitialized()
        {
            UpdateDateTime();

            // Configurar temporizador para gatillar la actualización cada 1000ms (1 segundo)
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        /// <summary>
        /// Maneja el callback periódico del temporizador. Utiliza InvokeAsync para sincronizar la UI en el hilo principal.
        /// </summary>
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                UpdateDateTime();
                StateHasChanged();
            });
        }

        /// <summary>
        /// Formatea e inyecta la marca temporal actual con formato de cultura peruana ("es-PE").
        /// </summary>
        private void UpdateDateTime()
        {
            var now = DateTime.Now;
            string datePart = now.ToString("dddd, d 'de' MMMM 'de' yyyy", _peCulture);
            string timePart = now.ToString("h:mm:ss tt", _peCulture);
            
            // Capitalizar primera letra del día de la semana
            if (!string.IsNullOrEmpty(datePart))
            {
                datePart = char.ToUpper(datePart[0]) + datePart.Substring(1);
            }

            CurrentDateTimeString = $"{datePart}, {timePart}";
        }

        /// <summary>
        /// Libera los recursos del temporizador al destruirse el componente para evitar fugas de memoria.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
