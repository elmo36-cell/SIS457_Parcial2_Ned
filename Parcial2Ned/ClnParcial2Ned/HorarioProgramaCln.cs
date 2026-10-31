using CadParcial2Ned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnParcial2Ned
{
    public class HorarioProgramaCln
    {
        public static int crear(HorarioPrograma horario)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.HorarioPrograma.Where(x => x.titulo == horario.titulo && x.estado != (short)Estado.Eliminado).FirstOrDefault();
                if (existe != null) throw new Exception($"El programa '{horario.titulo}' ya existe");

                horario.fechaRegistro = DateTime.Now;
                horario.estado = (short)Estado.Activo;

                context.HorarioPrograma.Add(horario);
                context.SaveChanges();
                return horario.id;
            }
        }

        public static int actualizar(HorarioPrograma horario)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.HorarioPrograma.Find(horario.id);
                if (existe == null) throw new Exception($"El horario con id {horario.id} no existe");

                existe.idCanal = horario.idCanal;
                existe.titulo = horario.titulo;
                existe.horaInicio = horario.horaInicio;
                existe.horaFin = horario.horaFin;
                existe.diaSemana = horario.diaSemana;
                existe.conductorPrincipal = horario.conductorPrincipal;
                existe.usuarioRegistro = horario.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new Parcial2NedEntities())
            {
                var existe = context.HorarioPrograma.Find(id);
                if (existe == null) throw new Exception($"El horario con id {id} no existe");

                existe.estado = (short)Estado.Eliminado;
                existe.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static HorarioPrograma obtenerUno(int id)
        {
            using (var context = new Parcial2NedEntities())
            {
                var horario = context.HorarioPrograma.Find(id);
                if (horario == null) throw new Exception($"El horario con id {id} no existe");
                return horario;
            }
        }

        public static List<HorarioPrograma> listar()
        {
            using (var context = new Parcial2NedEntities())
            {
                return context.HorarioPrograma.Where(x => x.estado == (short)Estado.Activo).OrderBy(x => x.titulo).ToList();
            }
        }

        public static List<paHorarioProgramaListar_Result> listarPa(string parametro)
        {
            using (var context = new Parcial2NedEntities())
            {
                if (string.IsNullOrEmpty(parametro)) parametro = "";
                return context.paHorarioProgramaListar(parametro.Trim()).ToList();
            }
        }
    }
}
