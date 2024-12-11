using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{

    /// <summary>
    /// Benutzerdefinierter Basiskontroller für einheitliche API-Antworten.
    /// </summary>
    public class CustomBaseController : ControllerBase
    {

        /// <summary>
        /// Gibt eine einheitliche API-Antwort zurück, basierend auf dem generischen Response-Objekt.
        /// </summary>
        /// <typeparam name="T">Typ der Daten, die in der Antwort enthalten sind.</typeparam>
        /// <param name="response">Antwortobjekt mit Statuscode und Daten.</param>
        /// <returns>Ein HTTP-Objekt mit dem Antwortinhalt und dem Statuscode.</returns>
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            // EINTRAGEN using AuthServer.DTOs;

            return new ObjectResult(response) // Erstellt eine HTTP-Antwort mit dem gegebenen Inhalt
            {
                StatusCode = response.StatusCode // Setzt den HTTP-Statuscode entsprechend dem Response-Objekt
            };
        }


    }
}
