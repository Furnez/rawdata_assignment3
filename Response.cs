using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Assignment_3
{
    public class Response
    {
        public string Status { get; set; }
        public string Body { get; set; }
        public void MethodCheck(ref Request request)
        {
            var method_ok = new string[] { "create", "delete", "update", "read", "echo" };
            if (!method_ok.Contains(request.Method))
            {
                this.Status = "illegal method";
            }
            else
            {
                if (!this.CheckMissingProperty(ref request))
                {
                    if (!this.CheckNotUnixDate(ref request))
                    {
                        switch (request.Method)
                        {
                            case "create":
                                this.MethodCreate(ref request);
                                break;
                            case "read":
                                this.MethodRead(ref request);
                                break;
                            case "update":
                                this.MethodUpdate(ref request);
                                break;
                            case "delete":
                                this.MethodDelete(ref request);
                                break;
                            case "echo":
                                this.MethodEcho(ref request);
                                break;
                        }
                    }
                }
            }
        }
        private void MethodCreate(ref Request request)
        {
            if (!this.CheckWrongBody(ref request))
            {
                var body_obj = JsonConvert.DeserializeObject<Category>(request.Body);
                if (request.Path == "/api/categories") {
                    var name = body_obj.GetType().GetProperty("name").GetValue(body_obj);
                    this.Status = "2 Created";
                    this.Body = JsonConvert.SerializeObject(new Category(name.ToString()));
                } else {
                    this.Status = "4 Bad request";
                }
            }
        }
        private void MethodRead(ref Request request)
        {

        }

        private void MethodUpdate(ref Request request)
        {
            if (!this.CheckWrongBody(ref request))
            {
                
            }
        }
        private void MethodDelete(ref Request request)
        {

        }
        private void MethodEcho(ref Request request)
        {
            this.Status = "1 Ok";
            this.Body = request.Body;
        }
        private bool CheckMissingProperty(ref Request request)
        {
            int count_null = 0;
            List<string> array_ms_properties = new List<string>();
            foreach (PropertyInfo pi in request.GetType().GetProperties())
            {
                if (pi.GetValue(request) == null)
                {
                    this.Status = "missing " + pi.Name.ToLower();
                    array_ms_properties.Add(pi.Name);
                }
            }
            count_null = array_ms_properties.ToArray().Length;
            if (count_null == 1)
            {
                if (array_ms_properties.ToArray()[0] == "Body")
                {
                    if (request.Method != "create" && request.Method != "update" && request.Method != "echo")
                    {
                        this.Status = null;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (array_ms_properties.ToArray()[0] == "Path") {
                    if (request.Method == "echo") {
                        this.Status = null;
                        return false;
                    } else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            else if (count_null > 1)
            {
                this.Status = "missing resource";
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckNotUnixDate(ref Request request)
        {
            int unix_date;
            if (int.TryParse(request.Date, out unix_date))
            {
                return false;
            }
            else
            {
                this.Status = "illegal date";
                return true;
            }
        }
        private bool CheckWrongBody(ref Request request)
        {
            if (request.Method == "create") {
                if (request.Body.GetType().GetProperty("name") == null) {
                    this.Status = "illegal body";
                    return true;
                } else {
                    return false;
                }
            } else if (request.Method == "update") {
                if (request.Body.GetType().GetProperty("cid") == null || request.Body.GetType().GetProperty("name") == null) {
                    this.Status = "illegal body";
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
    }
}