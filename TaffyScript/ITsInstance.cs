using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents an instance of an object in TaffyScript.
    /// </summary>
    public interface ITsInstance
    {

        /// <summary>
        /// Gets or sets a value based on a variable name.
        /// </summary>
        /// <param name="memberName">The name of the variable</param>
        /// <returns></returns>
        TsObject this[string memberName] { get; set; }

        /// <summary>
        /// Gets the type of this instance.
        /// </summary>
        string ObjectType { get; }

        /// <summary>
        /// Gets the value of a member from this instance.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <returns></returns>
        TsObject GetMember(string name);

        /// <summary>
        /// Sets the value of a member on this instance.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <param name="value">The new value.</param>
        void SetMember(string name, TsObject value);

        /// <summary>
        /// Gets a delegate defined by this instance.
        /// </summary>
        /// <param name="delegateName">The name of the delegate.</param>
        /// <param name="del">If found, the delegate.</param>
        /// <returns>True if found, false otherwise.</returns>
        bool TryGetDelegate(string delegateName, out TsDelegate del);

        /// <summary>
        /// Gets a delegate defined by this instance.
        /// </summary>
        /// <param name="delegateName">The name of the delegate.</param>
        /// <returns></returns>
        TsDelegate GetDelegate(string delegateName);

        /// <summary>
        /// Calls a script defined or assigned to the instance.
        /// </summary>
        /// <param name="scriptName">The name of the script to call.</param>
        /// <param name="args">Any arguments to pass to the script.</param>
        /// <returns>Script result.</returns>
        TsObject Call(string scriptName, params TsObject[] args);
    }
}
