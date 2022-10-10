using System;
using DotLiquid;

namespace Server.Utils
{
	public class PageTemplate
	{
		private String _path;

		/// <summary>
		///   An HTML document template
		/// </summary>
		public PageTemplate(String path)
		{
            if (!path.EndsWith(".html"))
			{
				path += ".html";
			}

            this._path = Path.Join("html", path);
		}

		/// <summary>
		///   Render the template using <param>drop</param> as the data source.
		/// </summary>
		public String render(object? drop = null)
		{
			Template template;
			using (StreamReader f = new StreamReader(this._path))
			{
				template = Template.Parse(f.ReadToEnd());
			}

            if (drop == null)
			{
				return template.Render();
			}
			else
			{
				return template.Render(Hash.FromAnonymousObject(drop));
			}
		}
	}
}