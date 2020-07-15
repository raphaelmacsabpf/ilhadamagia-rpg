using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Server
{
    public class DebugResolveModule : Module
    {
        private readonly ThreadLocal<ResolveInfo> _current = new ThreadLocal<ResolveInfo>();

        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += Registration_Preparing;
            registration.Activating += Registration_Activating;

            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        private void Registration_Preparing(object sender, PreparingEventArgs e)
        {
            _current.Value = new ResolveInfo(e.Component.Activator.LimitType, _current.Value);
        }

        private void Registration_Activating(object sender, ActivatingEventArgs<object> e)
        {
            var current = _current.Value;
            current.MarkComponentAsResolved();
            _current.Value = current.Parent;

            if (current.Parent == null)
            {
                VisualizeGraph(current);
                Console.WriteLine(
                    $"[IM AppBootstrap] Total resolve time: {current.ResolveTime.TotalMilliseconds} ms.");
            }
        }

        private static void VisualizeGraph(ResolveInfo node, int depth = 0)
        {
            Console.Write("[IM AppBootstrap] ");
            for (int i = 0; i < depth; i++)
            {
                Console.Write("   ");
            }

            Console.Write(node.ComponentType);
            Console.Write(" (");
            Console.Write(node.ResolveTime.TotalMilliseconds.ToString("F1"));
            Console.Write(" ms. / ");
            Console.Write(node.CreationTime.TotalMilliseconds.ToString("F1"));
            Console.Write(" ms.)");

            Console.WriteLine("");

            foreach (var dependency in node.Dependencies)
            {
                VisualizeGraph(dependency, depth + 1);
            }
        }

        private sealed class ResolveInfo
        {
            private Stopwatch _watch = Stopwatch.StartNew();

            public ResolveInfo(Type componentType, ResolveInfo parent)
            {
                ComponentType = componentType;
                Parent = parent;
                Dependencies = new List<ResolveInfo>(4);

                if (parent != null)
                {
                    parent.Dependencies.Add(this);
                }
            }

            public Type ComponentType { get; }

            // Time it took to create the type including its dependencies
            public TimeSpan ResolveTime { get; private set; }

            // Time it took to create the type excluding its dependencies
            public TimeSpan CreationTime { get; private set; }

            public ResolveInfo Parent { get; }

            public List<ResolveInfo> Dependencies { get; }

            public void MarkComponentAsResolved()
            {
                ResolveTime = _watch.Elapsed;
                CreationTime = ResolveTime;

                foreach (var dependency in this.Dependencies)
                {
                    CreationTime -= dependency.ResolveTime;
                }

                _watch = null;
            }
        }
    }
}