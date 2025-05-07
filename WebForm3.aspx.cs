using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wsCheckUsuario.Models;

namespace wsCheckUsuario
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Vaidación de 1er carga de páhina (postBack)
            if (Page.IsPostBack == false)
            {
                //Llamada patra ejecucion del metodo 
                await cargaDatosTipoUsuario();
            }

        }
        // Creación del método asíncrono para ejecutar el
        // endpoint vwTipoUsuario
        private async Task cargaDatosTipoUsuario()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Configuración de la peticion HTTP
                    string apiUrl = "https://localhost:44335/check/usuario/vwtipousuario";
                    // Ejecución del endpoint
                    HttpResponseMessage respuesta = await client.GetAsync(apiUrl);
                    // ---------------------------------------------------
                    // Validación de recepción de respuesta Json
                    clsApiStatus objRespuesta = new clsApiStatus();

                    // Validación del estatus OK
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        objRespuesta = JsonConvert.DeserializeObject<clsApiStatus>(resultado);
                        // ------------------------------------------
                        JArray jsonArray = (JArray)objRespuesta.datos["vwTipoUsuario"];
                        // Convertir JArray a DataTable
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonArray.ToString());
                        // -------------------------------------------
                        // Visualización de los datos formateados DropDownList
                        DropDownList1.DataSource = dt;
                        DropDownList1.DataTextField = "descripcion";
                        DropDownList1.DataValueField = "clave";
                        DropDownList1.DataBind();
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>" +
                                       "alert('Error de conexión con el servicio');" +
                                       "</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='javascript'>" +
                               "alert('Error de la aplicación, intentar nuevamente');" +
                               "</script>");
            }
        }

        // Creación del método asíncrono para ejecutar el
        // endpoint spInsUsuario
        private async Task cargaDatos()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Configuración del Json que se enviará
                    String data = @"{
                                  ""nombre"":""" + TextBox2.Text + "\"," +
                                  "\"apellidoPaterno\":\"" + TextBox3.Text + "\"," +
                                  "\"apellidoMaterno\":\"" + TextBox4.Text + "\"," +
                                  "\"usuario\":\"" + TextBox5.Text + "\"," +
                                  "\"contrasena\":\"" + TextBox6.Text + "\"," +
                                  "\"ruta\":\"" + TextBox7.Text + "\"," +
                                  "\"tipo\":\"" + DropDownList1.SelectedValue + "\"" +
                                  "}";
                    // Configuración del contenido del <body> a enviar
                    HttpContent contenido = new StringContent
                                (data, Encoding.UTF8, "application/json");
                    // Ejecución de la petición HTTP
                    string apiUrl = "https://localhost:44335/check/usuario/spinsusuario";
                    // ----------------------------------------------
                    HttpResponseMessage respuesta =
                        await client.PostAsync(apiUrl, contenido);
                    // ---------------------------------------------------
                    // Validación de recepción de respuesta Json
                    clsApiStatus objRespuesta = new clsApiStatus();
                    // ---------------------------------------------------

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado =
                                await respuesta.Content.ReadAsStringAsync();
                        objRespuesta = JsonConvert.DeserializeObject<clsApiStatus>(resultado);

                        // Bandera de estatus del proceso
                        if (objRespuesta.ban == 0)
                        {
                            Response.Write("<script language='javascript'>" +
                                           "alert('Usuario registrado exitosamente');" +
                                           "</script>");
                            Response.Write("<script language='javascript'>" +
                                           "document.location.href='WebForm2.aspx';" +
                                           "</script>");
                        }
                        if (objRespuesta.ban == 1)
                        {
                            Response.Write("<script language='javascript'>" +
                                           "alert('El nombre de usuario ya existe');" +
                                           "</script>");
                        }
                        if (objRespuesta.ban == 2)
                        {
                            Response.Write("<script language='javascript'>" +
                                           "alert('El usuario ya existe');" +
                                           "</script>");
                        }
                        if (objRespuesta.ban == 3)
                        {
                            Response.Write("<script language='javascript'>" +
                                           "alert('El tipo de usuario no existe');" +
                                           "</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>" +
                                       "alert('Error de conexión con el servicio');" +
                                       "</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='javascript'>" +
                               "alert('Error de la aplicación, intentar nuevamente');" +
                               "</script>");
            }
        }




        protected async void Button1_Click(object sender, EventArgs e)
        {
            // Nombre
            if (TextBox2.Text == "")
            {
                Response.Write("<script language='javascript'>" +
                              "alert('El nombre está vacio');" +
                              "</script>");
            }
            else
            {   //apellido paterno
                if (TextBox3.Text == "")
                {
                    Response.Write("<script language='javascript'>" +
                                  "alert('El apellido paterno está vacio');" +
                                  "</script>");
                }

                else
                {   //apellido materno
                    if (TextBox4.Text == "")
                    {
                        Response.Write("<script language='javascript'>" +
                                      "alert('El apellido materno está vacio');" +
                                      "</script>");
                    }


                    else
                    {   //usuario
                        if (TextBox5.Text == "")
                        {
                            Response.Write("<script language='javascript'>" +
                                          "alert('El usuario está vacio');" +
                                          "</script>");
                        }
                        else
                        {   // contraseña
                            if (TextBox6.Text == "")
                            {
                                Response.Write("<script language='javascript'>" +
                                              "alert('La contraseña está vacio');" +
                                              "</script>");
                            }
                            else
                            {   // ruta foto
                                if (TextBox7.Text == "")
                                {
                                    Response.Write("<script language='javascript'>" +
                                                  "alert('La ruta está vacio');" +
                                                  "</script>");
                                }
                                else
                                {
                                    //Ejecucion asincrona del metodo de insercion de usuario
                                    await cargaDatos();
                                }

                            }
                        }
                    }
                }
            }
        }
        //Metodo para buscar el usuario por clave 
        

        //Metodo de limpiar el formulario 
        private void LimpiarFormulario()
        {
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            DropDownList1.ClearSelection();
        }

        protected async void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {
            string clave = TextBox1.Text.Trim();
            if (string.IsNullOrEmpty(clave))
            {
                Response.Write("<script>alert('Ingresa una clave.');</script>");
                return;
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:44335/check/usuario/spbuscarusuarioclave?clave={clave}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        clsApiStatus objRespuesta = JsonConvert.DeserializeObject<clsApiStatus>(jsonResult);

                        if (objRespuesta.ban == 1)
                        {
                            JArray arreglo = (JArray)objRespuesta.datos["usuario"];
                            JObject u = (JObject)arreglo[0];

                            TextBox2.Text = u["USU_NOMBRE"].ToString();
                            TextBox3.Text = u["USU_APELLIDO_PATERNO"].ToString();
                            TextBox4.Text = u["USU_APELLIDO_MATERNO"].ToString();
                            TextBox5.Text = u["USU_USUARIO"].ToString();
                            TextBox6.Text = u["USU_CONTRASENA"].ToString();
                            TextBox7.Text = u["USU_RUTA"].ToString();
                            DropDownList1.SelectedValue = u["TIP_CVE_TIPOUSUARIO"].ToString();
                        }
                        else
                        {
                            LimpiarFormulario();
                            Response.Write("<script>alert('Usuario no encontrado');</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Error al conectar con el API');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }

        }

        protected async void Button2_Click(object sender, EventArgs e)
        {
            string clave = TextBox1.Text.Trim();

            if (string.IsNullOrEmpty(clave))
            {
                Response.Write("<script>alert('Ingresa la clave del usuario a modificar');</script>");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:44335/check/usuario/spupdusuario";

                    var datos = new
                    {
                        cve = clave,
                        nombre = TextBox2.Text,
                        apellidoPaterno = TextBox3.Text,
                        apellidoMaterno = TextBox4.Text,
                        usuario = TextBox5.Text,
                        contrasena = TextBox6.Text,
                        ruta = TextBox7.Text,
                        tipo = DropDownList1.SelectedValue
                    };

                    string json = JsonConvert.SerializeObject(datos);
                    HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage respuesta = await client.PutAsync(apiUrl, contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        clsApiStatus objRespuesta = JsonConvert.DeserializeObject<clsApiStatus>(resultado);

                        string mensaje = objRespuesta.datos["msgData"].ToString();
                        Response.Write($"<script>alert('{mensaje}');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error al conectar con el API');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

        protected async void Button3_Click(object sender, EventArgs e)
        {
            string clave = TextBox1.Text.Trim();

            if (string.IsNullOrEmpty(clave))
            {
                Response.Write("<script>alert('Ingresa una clave para eliminar.');</script>");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:44335/check/usuario/spdelusuario?clave={clave}";
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(apiUrl)
                    };

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        clsApiStatus objRespuesta = JsonConvert.DeserializeObject<clsApiStatus>(jsonResult);

                        string mensaje = objRespuesta.datos["msgData"].ToString();
                        Response.Write($"<script>alert('{mensaje}');</script>");

                        if (objRespuesta.ban == 1)
                        {
                            LimpiarFormulario();
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Error al conectar con el API.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }


        }

        //Metodo para modificar datos 



    }
}