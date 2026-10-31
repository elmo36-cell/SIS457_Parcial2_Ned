using CadParcial2Ned;
using ClnParcial2Ned;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Ned
{
    public partial class FrmPrograma : Form
    {
        private bool esNuevo;
        public FrmPrograma()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var programas = HorarioProgramaCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = programas;

            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idCanal"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;

            dgvLista.Columns["canal"].HeaderText = "Canal";
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["horaInicio"].HeaderText = "Hora Inicio";
            dgvLista.Columns["horaFin"].HeaderText = "Hora Fin";
            dgvLista.Columns["diaSemana"].HeaderText = "Día de Semana";
            dgvLista.Columns["conductorPrincipal"].HeaderText = "Conductor Principal";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";

            if (programas.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];

            btnEditar.Enabled = programas.Count > 0;
            btnEliminar.Enabled = programas.Count > 0;
        }

        private void cargarCanales()
        {
            cbxCanal.DataSource = CanalCln.listar();
            cbxCanal.ValueMember = "id";
            cbxCanal.DisplayMember = "nombre";
            cbxCanal.SelectedIndex = -1;
        }

        private void cargarDiasSemana()
        {
            cbxDiaSemana.Items.Clear();
            cbxDiaSemana.Items.Add("Lunes");
            cbxDiaSemana.Items.Add("Martes");
            cbxDiaSemana.Items.Add("Miércoles");
            cbxDiaSemana.Items.Add("Jueves");
            cbxDiaSemana.Items.Add("Viernes");
            cbxDiaSemana.Items.Add("Sábado");
            cbxDiaSemana.Items.Add("Domingo");
            
            cbxDiaSemana.SelectedIndex = -1;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void FrmPrograma_Load(object sender, EventArgs e)
        {
            Size = new Size(901, 357);
            listar();
            cargarCanales();
            cargarDiasSemana();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }
        private void resetearCampos()
        {
            cbxCanal.SelectedIndex = -1;
            txtTitulo.Clear();
            mtbHoraInicio.Clear();
            mtbHoraFin.Clear();
            cbxDiaSemana.SelectedIndex = -1;
            txtConductor.Clear();
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCanal.Clear();
            erpTitulo.Clear();
            erpConductor.Clear();
            erpCanal.Clear();
            erpTitulo.Clear();
            erpHoraInicio.Clear();
            erpHoraFin.Clear();
            erpDiaSemana.Clear();
            erpConductor.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(901, 476);
            pnlAcciones.Enabled = false;
            resetearCampos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(901, 476);
            pnlAcciones.Enabled = false;

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var horario = HorarioProgramaCln.obtenerUno(id);

            cbxCanal.SelectedValue = horario.idCanal;
            txtTitulo.Text = horario.titulo;
            mtbHoraInicio.Text = horario.horaInicio.ToString(@"hh\:mm");
            mtbHoraFin.Text = horario.horaFin.ToString(@"hh\:mm");
            cbxDiaSemana.Text = horario.diaSemana;
            txtConductor.Text = horario.conductorPrincipal;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(901, 357);
            pnlAcciones.Enabled = true;
        }
        private bool validarCampos()
        {
            bool esValido = true;
            resetearErrores();

            if (cbxCanal.SelectedIndex == -1)
            {
                erpCanal.SetError(cbxCanal, "Seleccione un canal obligatorio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                erpTitulo.SetError(txtTitulo, "El título del programa es obligatorio");
                esValido = false;
            }
            if (!mtbHoraInicio.MaskFull)
            {
                erpHoraInicio.SetError(mtbHoraInicio, "Ingrese una hora de inicio completa");
                esValido = false;
            }
            if (!mtbHoraFin.MaskFull)
            {
                erpHoraFin.SetError(mtbHoraFin, "Ingrese una hora de fin completa");
                esValido = false;
            }
            if (cbxDiaSemana.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cbxDiaSemana.Text))
            {
                erpDiaSemana.SetError(cbxDiaSemana, "Seleccione o escriba el día de la semana");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtConductor.Text))
            {
                erpConductor.SetError(txtConductor, "El conductor principal es obligatorio");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                var horario = new HorarioPrograma
                {
                    idCanal = (int)cbxCanal.SelectedValue,
                    titulo = txtTitulo.Text.Trim(),
                    horaInicio = TimeSpan.Parse(mtbHoraInicio.Text.Trim()),
                    horaFin = TimeSpan.Parse(mtbHoraFin.Text.Trim()),
                    diaSemana = cbxDiaSemana.Text.Trim(),
                    conductorPrincipal = txtConductor.Text.Trim(),
                    usuarioRegistro = "usrparcial2"
                };

                try
                {
                    if (esNuevo)
                    {
                        horario.fechaRegistro = DateTime.Now;
                        horario.estado = (short)Estado.Activo;
                        HorarioProgramaCln.crear(horario);
                    }
                    else
                    {
                        horario.id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                        HorarioProgramaCln.actualizar(horario);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Horario de programa guardado correctamente", "::: Mensaje - Parcial2Ned :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "::: Error - Parcial2Ned :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string titulo = dgvLista.CurrentRow.Cells["titulo"].Value.ToString();

            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja el programa '{titulo}'?",
                "::: Confirmación - Parcial2Ned :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                HorarioProgramaCln.eliminar(id, "usrparcial2");
                listar();
                MessageBox.Show("Programa dado de baja correctamente", "::: Mensaje - Parcial2Ned :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvLista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLista.Columns[e.ColumnIndex].Name == "horaInicio" || dgvLista.Columns[e.ColumnIndex].Name == "horaFin")
            {
                if (e.Value != null && e.Value is TimeSpan span)
                {
                    e.Value = span.ToString(@"hh\:mm");
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
