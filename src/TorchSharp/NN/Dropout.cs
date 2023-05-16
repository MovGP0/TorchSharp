// Copyright (c) .NET Foundation and Contributors.  All Rights Reserved.  See LICENSE in the project root for license information.
using System;
using static TorchSharp.torch;
using static TorchSharp.PInvoke.LibTorchSharp;

namespace TorchSharp
{
    using Modules;

    namespace Modules
    {
        /// <summary>
        /// This class is used to represent a dropout module.
        /// </summary>
        public sealed class Dropout : torch.nn.Module<Tensor, Tensor>
        {
            internal Dropout(double p = 0.5, bool inplace = false) : base(nameof(Dropout))
            {
                this.p = p;
                this.inplace = inplace;
            }

            /// <summary>
            /// Forward pass.
            /// </summary>
            /// <param name="tensor">Input tensor</param>
            /// <returns></returns>
            public override Tensor forward(Tensor tensor)
            {
                return torch.nn.functional.dropout(tensor, this.p, this.training, this.inplace);
            }

            // Rather than spending cycles only to discover that this module has neither
            // parameters nor buffers, just shortcut the move completely.
            protected internal override nn.Module _to(Device device, ScalarType dtype) => this;
            protected internal override nn.Module _to(DeviceType deviceType, int deviceIndex = -1) => this;
            protected internal override nn.Module _to(ScalarType dtype) => this;

            private bool inplace;
            private double p;
        }
    }

    public static partial class torch
    {
        public static partial class nn
        {
            /// <summary>
            /// During training, randomly zeroes some of the elements of the input tensor with probability p using samples from a Bernoulli distribution.
            /// Each channel will be zeroed out independently on every forward call.
            /// </summary>
            /// <param name="p">Probability of an element to be zeroed. Default: 0.5</param>
            /// <param name="inplace">If set to true, will do this operation in-place. Default: false</param>
            /// <returns></returns>
            public static Dropout Dropout(double p = 0.5, bool inplace = false)
            {
                return new Dropout(p, inplace);
            }

            public static partial class functional
            {
                /// <summary>
                /// During training, randomly zeroes some of the elements of the input tensor with probability p using samples from a Bernoulli distribution.
                /// Each channel will be zeroed out independently on every forward call.
                /// </summary>
                /// <returns></returns>
                public static Tensor dropout(Tensor input, double p = 0.5, bool training = true, bool inplace = false)
                {
                    var res = THSNN_dropout(input.Handle, p, training, inplace);
                    if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                    return new Tensor(res);
                }
            }
        }
    }
}
