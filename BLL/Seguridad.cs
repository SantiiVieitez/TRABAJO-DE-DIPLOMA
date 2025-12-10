using BE;
using SERVICIOS;
using System;
using System.Security.Cryptography;
using System.Text;

public class Seguridad
{
    public static string CalcularDVH(Factura factura)
    {
        string fechaFormateada = factura.Fecha.ToString("yyyy-MM-dd");
        string cadenaConcatenada =
            factura.ID.ToString() +
            factura.MetodoDePago.Trim() +     
            factura.DNI_Cliente.ToString() +  
            fechaFormateada;

        // Retornamos el hash de esa cadena
        return Encriptador.GetSHA256(cadenaConcatenada);
    }


}

