using System;
using System.Windows.Forms;
using System.Net.Mail;
using UserManagementWinApp.Utility;
using System.Threading.Tasks;
using UserManagement.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace UserManagementWinApp
{
    public partial class A : Form
    {
        UserAPI api = new UserAPI();
        const string UpdateBtnText = "Update User";
        const string CreateBtnText = "Add User";
        public A()
        {
            InitializeComponent();
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            if (await FormIsValid())
            {
                var user = new User
                {
                    Id = string.IsNullOrEmpty(tbxId.Text) ? 0 : int.Parse(tbxId.Text),
                    Name = tbxName.Text,
                    LastName = tbxLastName.Text,
                    Email = tbxEmail.Text
                };

                if (btnAddUser.Text == UpdateBtnText)
                {
                    await api.UpdateUser(user);

                    btnCancelEdit.Enabled = false;
                    btnAddUser.Text = CreateBtnText;
                }
                else
                {
                    await api.CreateUser(user);
                }

                ClearForm();
                await Load_dgvUsers();
            }
        }

        private async Task<bool> FormIsValid()
        {
            string errMsgEmpty = "The field {0} cannot be empty";
            lblErrorMsg.Text = string.Empty;

            var name = tbxName.Text;
            var lastName = tbxLastName.Text;
            var email = tbxEmail.Text;

            if (String.IsNullOrEmpty(name))
                lblErrorMsg.Text = string.Format(errMsgEmpty, lblName.Text);

            if (String.IsNullOrEmpty(lastName))
                lblErrorMsg.Text = string.Format(errMsgEmpty, lblName.Text);

            if (String.IsNullOrEmpty(email))
                lblErrorMsg.Text = string.Format(errMsgEmpty, lblName.Text);
            else if (!EmailIsValid(email))
                lblErrorMsg.Text = "The entered email is invalid";
            else if (!await api.EmailIsValid(email))
                lblErrorMsg.Text = "The entered email already exists in the system";

            if (string.IsNullOrEmpty(lblErrorMsg.Text))
                return true;

            return false;
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                _ = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        private async void ManageUserForm_Load(object sender, EventArgs e)
        {
            await Load_dgvUsers();
        }

        private async Task Load_dgvUsers()
        {
            var data = await api.GetAllUsers();
            dgvUsers.DataSource = data;

            if (dgvUsers.Columns.Count > 2)
            {
                dgvUsers.Columns["grdVEdit"].Visible = true;
                dgvUsers.Columns["grdVDelete"].Visible = true;
                dgvUsers.Columns["Id"].Visible = false;

                dgvUsers.Columns["Id"].DisplayIndex = 0;
                dgvUsers.Columns["Name"].DisplayIndex = 1;
                dgvUsers.Columns["LastName"].DisplayIndex = 2;
                dgvUsers.Columns["Email"].DisplayIndex = 3;
                dgvUsers.Columns["grdVEdit"].DisplayIndex = 4;
                dgvUsers.Columns["grdVDelete"].DisplayIndex = 5;
            }
            else
            {
                dgvUsers.Columns["grdVEdit"].Visible = false;
                dgvUsers.Columns["grdVDelete"].Visible = false;
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            var obj = senderGrid.Columns[e.ColumnIndex];
            var index = e.RowIndex;

            if (obj is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (obj.Name.Equals("grdVEdit"))
                {
                    grdVEdit_Click(index);
                }
                else if (obj.Name.Equals("grdVDelete"))
                {
                    grdVDelete_Click(index);
                }
            }
        }

        private void grdVEdit_Click(int rowIndex)
        {
            var users = (IEnumerable<User>)dgvUsers.DataSource;
            try
            {
                var user = users.ToList()[rowIndex];

                tbxId.Visible = true;
                lblId.Visible = true;

                tbxId.Text = user.Id.ToString();
                tbxName.Text = user.Name.ToString();
                tbxLastName.Text = user.LastName.ToString();
                tbxEmail.Text = user.Email.ToString();

                btnAddUser.Text = UpdateBtnText;
                btnCancelEdit.Enabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void grdVDelete_Click(int rowIndex)
        {
            var users = (IEnumerable<User>)dgvUsers.DataSource;
            try
            {
                var user = users.ToList()[rowIndex];

                ClearForm();
                await api.DeleteUser(user.Id);
                await Load_dgvUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            tbxId.Visible = false;
            lblId.Visible = false;

            tbxId.Text = string.Empty;
            tbxName.Text = string.Empty;
            tbxLastName.Text = string.Empty;
            tbxEmail.Text = string.Empty;
            btnAddUser.Text = CreateBtnText;
            btnCancelEdit.Enabled = false;
        }
    }
}
