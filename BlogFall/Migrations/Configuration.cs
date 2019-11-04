namespace BlogFall.Migrations
{
    using BlogFall.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogFall.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogFall.Models.ApplicationDbContext context)
        {
            #region Admin rol�n� ve kullan�c�s�n� olu�tur
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);//veritaban� ile olan ili�kiyi sa�lar bu y�zden �nce bu tan�mlan�r arka planda bu kullan�l�r
                var manager = new RoleManager<IdentityRole>(store);//rolleri �retmede bulmada kullan�l�yor
                var role = new IdentityRole { Name = "Admin" };//roller tablosunun entity kar��l��

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "busegarip96@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "busegarip96@gmail.com",
                    Email = "busegarip96@gmail.com"
                };

                manager.Create(user, "Ankara1.");//bu parola ile olu�tur.
                manager.AddToRole(user.Id, "Admin");//bu idli ki�iyi admin rol�ne at�yorum.

                //olu�turulan bu kullan�c�ya ait yaz�lar ekleyelim.
                #region Kategoriler ve Yaz�lar
                if (!context.Categories.Any())//hi� kategori yoksa
                {
                    Category cat1 = new Category
                    {
                        CategoryName = "Gezi Yaz�lar�"
                    };//gezi yaz�lar�
                    //https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
                    cat1.Posts = new List<Post>();//hashset daha uygun bir kez girilen bir daha girilemiyor
                    cat1.Posts.Add(new Post
                    {
                        Title = "Gezi Yaz�s� 1",
                        AuthorId = user.Id,
                        Content = @"
<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    cat1.Posts.Add(new Post
                    {
                        Title = "Gezi Yaz�s� 2",
                        AuthorId = user.Id,
                        Content = @"
<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    Category cat2 = new Category
                    {
                        CategoryName = "�� Yaz�lar�"
                    };//i� yaz�lar�
                    //https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
                    cat2.Posts = new List<Post>();
                    cat2.Posts.Add(new Post
                    {
                        Title = "�� Yaz�s� 1",
                        AuthorId = user.Id,
                        Content = @"
<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    cat2.Posts.Add(new Post
                    {
                        Title = "�� Yaz�s� 2",
                        AuthorId = user.Id,
                        Content = @"
<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);
                }

                #endregion
            }
            #endregion

            #region Admin kullan�c�s�na 77 yeni yaz� ekle
            if (!context.Categories.Any(x => x.CategoryName == "Di�er"))
            {
                ApplicationUser admin = context.Users.Where(x => x.UserName == "busegarip96@gmail.com").FirstOrDefault();

                if (admin != null)
                {
                    if (!context.Categories.Any(x => x.CategoryName == "Di�er"))
                    {
                        Category diger = new Category
                        {
                            CategoryName = "Di�er",
                            Posts = new HashSet<Post>()//ayn� objeyi bir daha ekleyememeye yarar tekrarlanmaz
                        };
                        for (int i = 1; i <= 77; i++)
                        {
                            diger.Posts.Add(new Post
                            {
                                Title = "Di�er Yaz� " + i,
                                AuthorId = admin.Id,
                                Content = @"
<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
",
                                CreationTime = DateTime.Now.AddMinutes(i)
                            });
                        }
                        context.Categories.Add(diger);
                    }
                }
            }
        }

        #endregion
    }
}

