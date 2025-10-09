using System.Collections.Generic;

namespace Steer73.RockIT.Templates.Dtos;

public class JobResponsePdfDto
{
    public SurveyDto Survey { get; set; } = null!;
    public IReadOnlyDictionary<string, object> ElementNameResponsePairs { get; set; } = new Dictionary<string, object>();
}

public class SurveyDto
{
	public string Description { get; set; } = null!;
	public string Name { get; set; } = null!;
    public List<PageDto> Pages { get; set; } = [];
}

public class PageDto
{
    public string Name { get; set; } = null!;
    public List<ElementDto> Elements { get; set; } = [];
}

public class ElementDto
{
	public string Description { get; set; } = null!;
	public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public List<ChoiceDto> Choices { get; set; } = [];
}

public class ChoiceDto
{
    public string Text { get; set; } = null!;
    public string Value { get; set; } = null!;
}