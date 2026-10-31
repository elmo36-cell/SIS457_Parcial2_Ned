using CadParcial2Ned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnParcial2Ned
{
    public class CanalCln
    {
        public static int crear(Canal canal)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.Canal.Where(x => x.nombre == canal.nombre && x.estado != (short)Estado.Eliminado).FirstOrDefault();
                if (existe != null) throw new Exception($"El canal '{canal.nombre}' ya existe");

                canal.fechaRegistro = DateTime.Now;
                canal.estado = (short)Estado.Activo;

                context.Canal.Add(canal);
                context.SaveChanges();
                return canal.id;
            }
        }

        public static int actualizar(Canal canal)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.Canal.Find(canal.id);
                if (existe == null) throw new Exception($"El canal con id {canal.id} no existe");

                existe.nombre = canal.nombre;
                existe.frecuencia = canal.frecuencia;
                existe.usuarioRegistro = canal.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.Canal.Find(id);
                if (existe == null) throw new Exception($"El canal con id {id} no existe");

                existe.estado = (short)Estado.Eliminado;
                existe.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static Canal obtenerUno(int id)
        {
            using (var context = new Parcial2NedEntities())
            {
                var canal = context.Canal.Find(id);
                if (canal == null) throw new Exception($"El canal con id {id} no existe");
                return canal;
            }
        }

        public static List<Canal> listar()
        {
            using (var context = new Parcial2NedEntities())
            {
                return context.Canal.Where(x => x.estado == (short)Estado.Activo).OrderBy(x => x.nombre).ToList();
            }
        }

        public static List<paCanalListar_Result> listarPa(string parametro)
        {
            using (var context = new Parcial2NedEntities())
            {
                if (string.IsNullOrEmpty(parametro)) parametro = "";
                return context.paCanalListar(parametro.Trim()).ToList();
            }
        }
    }
}
