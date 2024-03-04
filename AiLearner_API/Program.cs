using AiLearner_API.Services;
using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.dbContext;
using Microsoft.EntityFrameworkCore;


namespace AiLearner_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AiLearnerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AiLearnerConnection")));
            builder.Services.AddSingleton<OpenAIService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            /**/
            //add my ExceptionMiddleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapGet("/OpenAi", async (OpenAIService openAIService) =>
            {
                string message = "Built-in directives\r\nDirectives are classes that add additional behavior to elements in your Angular applications. Use Angular's built-in directives to manage forms, lists, styles, and what users see.\r\n\r\nSee the live example / download example for a working example containing the code snippets in this guide.\r\n\r\nThe different types of Angular directives are as follows:\r\n\r\nDIRECTIVE TYPES\tDETAILS\r\nComponents\tUsed with a template. This type of directive is the most common directive type.\r\nAttribute directives\tChange the appearance or behavior of an element, component, or another directive.\r\nStructural directives\tChange the DOM layout by adding and removing DOM elements.\r\nThis guide covers built-in attribute directives and structural directives.\r\n\r\n\r\nBuilt-in attribute directives\r\nAttribute directives listen to and modify the behavior of other HTML elements, attributes, properties, and components.\r\n\r\nMany attribute directives are defined through modules such as the CommonModule, RouterModule and FormsModule.\r\n\r\nThe most common attribute directives are as follows:\r\n\r\nCOMMON DIRECTIVES\tDETAILS\r\nNgClass\tAdds and removes a set of CSS classes.\r\nNgStyle\tAdds and removes a set of HTML styles.\r\nNgModel\tAdds two-way data binding to an HTML form element.\r\nBuilt-in directives use only public APIs. They do not have special access to any private APIs that other directives can't access.\r\n\r\n\r\nAdding and removing classes with NgClass\r\nAdd or remove multiple CSS classes simultaneously with ngClass.\r\n\r\nTo add or remove a single class, use class binding rather than NgClass.\r\n\r\nImport CommonModule in the component\r\nTo use NgClass, import CommonModule and add it to the component's imports list.\r\n\r\nsrc/app/app.component.ts (CommonModule import)\r\ncontent_copy\r\nimport {CommonModule} from '@angular/common';\r\n/* . . . */\r\n@Component({\r\n  standalone: true,\r\n  /* . . . */\r\n  imports: [\r\n    CommonModule, // <-- import into the component\r\n    /* . . . */\r\n  ],\r\n})\r\nexport class AppComponent implements OnInit {\r\n  /* . . . */\r\n}\r\nUsing NgClass with an expression\r\nOn the element you'd like to style, add [ngClass] and set it equal to an expression. In this case, isSpecial is a boolean set to true in app.component.ts. Because isSpecial is true, ngClass applies the class of special to the <div>.\r\n\r\nsrc/app/app.component.html\r\ncontent_copy\r\n<!-- toggle the \"special\" class on/off with a property -->\r\n<div [ngClass]=\"isSpecial ? 'special' : ''\">This div is special</div>\r\nUsing NgClass with a method\r\nTo use NgClass with a method, add the method to the component class. In the following example, setCurrentClasses() sets the property currentClasses with an object that adds or removes three classes based on the true or false state of three other component properties.\r\n\r\nEach key of the object is a CSS class name. If a key is true, ngClass adds the class. If a key is false, ngClass removes the class.\r\n\r\nsrc/app/app.component.ts\r\ncontent_copy\r\ncurrentClasses: Record<string, boolean> = {};\r\n/* . . . */\r\nsetCurrentClasses() {\r\n  // CSS classes: added/removed per current state of component properties\r\n  this.currentClasses = {\r\n    saveable: this.canSave,\r\n    modified: !this.isUnchanged,\r\n    special: this.isSpecial,\r\n  };\r\n}\r\nIn the template, add the ngClass property binding to currentClasses to set the element's classes:\r\n\r\nsrc/app/app.component.html\r\ncontent_copy\r\n<div [ngClass]=\"currentClasses\">This div is initially saveable, unchanged, and special.</div>\r\nFor this use case, Angular applies the classes on initialization and in case of changes. The full example calls setCurrentClasses() initially with ngOnInit() and when the dependent properties change through a button click. These steps are not necessary to implement ngClass. For more information, see the live example / download example app.component.ts and app.component.html.\r\n\r\n\r\nSetting inline styles with NgStyle\r\nImport CommonModule in the component\r\nTo use NgStyle, import CommonModule and add it to the component's imports list.\r\n\r\nsrc/app/app.component.ts (CommonModule import)\r\ncontent_copy\r\nimport {CommonModule} from '@angular/common';\r\n/* . . . */\r\n@Component({\r\n  standalone: true,\r\n  /* . . . */\r\n  imports: [\r\n    CommonModule, // <-- import into the component\r\n    /* . . . */\r\n  ],\r\n})\r\nexport class AppComponent implements OnInit {\r\n  /* . . . */\r\n}\r\nUsing NgStyle in your component\r\nUse NgStyle to set multiple inline styles simultaneously, based on the state of the component.\r\n\r\nTo use NgStyle, add a method to the component class.\r\n\r\nIn the following example, setCurrentStyles() sets the property currentStyles with an object that defines three styles, based on the state of three other component properties.\r\n\r\nsrc/app/app.component.ts\r\ncontent_copy\r\ncurrentStyles: Record<string, string> = {};\r\n/* . . . */\r\nsetCurrentStyles() {\r\n  // CSS styles: set per current state of component properties\r\n  this.currentStyles = {\r\n    'font-style': this.canSave ? 'italic' : 'normal',\r\n    'font-weight': !this.isUnchanged ? 'bold' : 'normal',\r\n    'font-size': this.isSpecial ? '24px' : '12px',\r\n  };\r\n}\r\nTo set the element's styles, add an ngStyle property binding to currentStyles.\r\n\r\nsrc/app/app.component.html\r\ncontent_copy\r\n<div [ngStyle]=\"currentStyles\">\r\n  This div is initially italic, normal weight, and extra large (24px).\r\n</div>\r\nFor this use case, Angular applies the styles upon initialization and in case of changes. To do this, the full example calls setCurrentStyles() initially with ngOnInit() and when the dependent properties change through a button click. However, these steps are not necessary to implement ngStyle on its own. See the live example / download example app.component.ts and app.component.html for this optional implementation.\r\n\r\n\r\nDisplaying and updating properties with ngModel\r\nUse the NgModel directive to display a data property and update that property when the user makes changes.\r\n\r\nImport FormsModule and add it to the AppComponent's imports list.\r\n\r\nsrc/app/app.component.ts (FormsModule import)\r\ncontent_copy\r\nimport {FormsModule} from '@angular/forms'; // <--- JavaScript import from Angular\r\n/* . . . */\r\n@Component({\r\n  standalone: true,\r\n  /* . . . */\r\n  imports: [\r\n    CommonModule, // <-- import into the component\r\n    FormsModule, // <--- import into the component\r\n    /* . . . */\r\n  ],\r\n})\r\nexport class AppComponent implements OnInit {\r\n  /* . . . */\r\n}\r\nAdd an [(ngModel)] binding on an HTML <form> element and set it equal to the property, here name.\r\n\r\nsrc/app/app.component.html (NgModel example)\r\ncontent_copy\r\n<label for=\"example-ngModel\">[(ngModel)]:</label>\r\n<input [(ngModel)]=\"currentItem.name\" id=\"example-ngModel\">\r\nThis [(ngModel)] syntax can only set a data-bound property.\r\n\r\nTo customize your configuration, write the expanded form, which separates the property and event binding. Use property binding to set the property and event binding to respond to changes. The following example changes the <input> value to uppercase:\r\n\r\nsrc/app/app.component.html\r\ncontent_copy\r\n<input [ngModel]=\"currentItem.name\" (ngModelChange)=\"setUppercaseName($event)\" id=\"example-uppercase\">\r\nHere are all variations in action, including the uppercase version:\r\n\r\nNgModel variations\r\nNgModel and value accessors\r\nThe NgModel directive works for an element supported by a ControlValueAccessor. Angular provides value accessors for all of the basic HTML form elements. For more information, see Forms.\r\n\r\nTo apply [(ngModel)] to a non-form built-in element or a third-party custom component, you have to write a value accessor. For more information, see the API documentation on DefaultValueAccessor.\r\n\r\nWhen you write an Angular component, you don't need a value accessor or NgModel if you name the value and event properties according to Angular's two-way binding syntax.";
                var result = await openAIService.CallChatGPTAsync(message , 5);
                return Results.Ok(result);
            });

            /**/
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
