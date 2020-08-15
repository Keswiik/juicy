﻿using Juicy.Interfaces.Binding;
using Juicy.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Juicy.Inject.Binding {

    /// <inheritdoc cref="ICollectionBinding"/>
    public sealed class CollectionBinding : Binding, ICollectionBinding {

        public List<Type> ImplementationTypes { get; }

        /// <summary>
        /// Consumes builder to fill out attributes.
        /// </summary>
        /// <param name="component">The component to pull method information from.</param>
        private CollectionBinding(ICollectionBindingComponent component) : base(component) {
            ImplementationTypes = component.ImplementationTypes;
        }

        #region Builder

        /// <inheritdoc/>
        private interface ICollectionBindingComponent : IBindingBuilderComponent {
            internal List<Type> ImplementationTypes { get; }
        }

        /// <inheritdoc/>
        public class CollectionBindingComponent<T> : BindingComponent<T>, ICollectionBindingComponent where T : CollectionBindingComponent<T> {

            List<Type> ICollectionBindingComponent.ImplementationTypes => ImplementationTypes;

            public List<Type> ImplementationTypes { get; }

            internal CollectionBindingComponent(Type type, IModule module) : base(type, module) {
                ImplementationTypes = new List<Type>();
            }

            public T To<J>() {
                ImplementationTypes.Add(typeof(J));
                return this as T;
            }
        }

        /// <summary>
        /// Builder used to produce new collection bindings.
        /// </summary>
        public class CollectionBindingBuilder : CollectionBindingComponent<CollectionBindingBuilder>, IBuilder {
            internal CollectionBindingBuilder(Type type, IModule module) : base(type, module) { }

            IBinding IBuilder.Build() {
                return new CollectionBinding(this);
            }
        }

        #endregion
    }
}
