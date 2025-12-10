using System;

public class BackUpBLL
{
    BackUpDAL da;

    public BackUpBLL()
    {
        da = BackUpDAL.Instance;
    }

    public void RealizarBackUp(string ruta, string nombreBase)
    {
        if (string.IsNullOrEmpty(ruta)) throw new Exception("La ruta es obligatoria");
        if (string.IsNullOrEmpty(nombreBase)) throw new Exception("El nombre de la BD es obligatorio");

        da.RealizarBackUp(ruta, nombreBase);
    }

    public void RestaurarBackUp(string ruta, string nombreBase)
    {
        if (string.IsNullOrEmpty(ruta)) throw new Exception("La ruta es obligatoria");
        if (string.IsNullOrEmpty(nombreBase)) throw new Exception("El nombre de la BD es obligatorio");

        da.RestaurarBackUp(ruta, nombreBase);
    }
}