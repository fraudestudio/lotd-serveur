using System;
using DotLiquid;

namespace PageTemplate
{
	public class PageTemplate
	{
		private String _path;

		/// <summary>
		///   An HTML document template
		/// </summary>
		public PageTemplate(String path)
		{
			this._path = path;
		}

		/// <summary>
		///   Render the template using <param>drop</param> as the data source.
		/// </summary>
		public String render(Drop drop = null)
		{
			Template template;
			using (FileStream f = new FileStream(this._path, ))
			{
				template = Template.Parse(f.ReadToEnd());
			}

			if (drop == null)
			{
				return template.Render();
			}
			else
			{
				return template.Render(drop);
			}
		}
	}
}