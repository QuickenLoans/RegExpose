using AutoMapper;
using RegExpose.Nodes;
using RegExpose.Nodes.Alternation;
using System.Collections.Generic;

namespace RegExpose.Api.Services
{
    public static class DtoMapper
    {
        public static void Init()
        {
            Mapper.CreateMap<RegexNode, RegexNodeDto>()
                .ForMember(
                    destination => destination.Children,
                    options => options.ResolveUsing(
                        node =>
                        {
                            var leaf = node as LeafNode;
                            if (leaf != null)
                            {
                                return null;
                            }

                            var children = new List<RegexNodeDto>();

                            var container = node as ContainerNode;
                            if (container != null)
                            {
                                foreach (var child in container.Children)
                                {
                                    children.Add(Mapper.Map<RegexNodeDto>(child));
                                }
                                return children;
                            }

                            var wrapper = node as WrapperNode;
                            if (wrapper != null)
                            {
                                children.Add(Mapper.Map<RegexNodeDto>(wrapper.Child));
                                return children;
                            }

                            var alternation = node as Alternation;
                            if (alternation != null)
                            {
                                foreach (var choice in alternation.Choices)
                                {
                                    children.Add(Mapper.Map<RegexNodeDto>(choice));
                                }
                                return children;
                            }

                            return children;
                        }));

            Mapper.CreateMap<ParseStep, ParseStepDto>()
                .ForMember(destination => destination.StepIndex, options => options.Ignore());

            Mapper.AssertConfigurationIsValid();
        }

        public static T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}