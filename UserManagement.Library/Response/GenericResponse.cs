using System.Collections.Generic;

namespace UserManagement.Library.Response
{
    public class GenericResponse : BaseResponse
    {
        public object Obj { get; private set; }
        public IEnumerable<object> ObjList { get; private set; }

        private GenericResponse(bool success, string message, object obj, IEnumerable<object> objList) : base(success, message)
        {
            Obj = obj;
            ObjList = objList;
        }

        /// <summary>
        /// Response for Not successful result.
        /// </summary>
        public GenericResponse(string message) : this(false, message, null, null)
        { }

        /// <summary>
        /// Response for correct processed but not successful result.
        /// </summary>
        public GenericResponse(bool result) : this(result, string.Empty, null, null)
        { }

        /// <summary>
        /// Response for GET-MULTIPLE.
        /// </summary>
        /// <param name="objList">
        /// Enumerable of Objects.
        /// </param>
        public GenericResponse(IEnumerable<object> objList) : this(true, string.Empty, null, objList)
        { }


        /// <summary>
        /// Response for GET.
        /// </summary>
        /// <param name="objList">
        /// Enumerable of Objects.
        /// </param>
        public GenericResponse(object obj) : this(true, string.Empty, obj, null)
        { }

        /// <summary>
        /// Response for PUT/DELETE.
        /// </summary>
        public GenericResponse() : this(true, string.Empty, null, null)
        { }
    }
}
