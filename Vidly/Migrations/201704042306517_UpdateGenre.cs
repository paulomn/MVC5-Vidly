namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGenre : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "GenreId", "dbo.Genres");
            DropPrimaryKey("dbo.Genres");
            DropColumn("dbo.Genres", "GenreId");
            AddColumn("dbo.Genres", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Genres", "Id");
            AddForeignKey("dbo.Movies", "GenreId", "dbo.Genres", "Id", cascadeDelete: true);

        }
        
        public override void Down()
        {
            AddColumn("dbo.Genres", "GenreId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Movies", "GenreId", "dbo.Genres");
            DropPrimaryKey("dbo.Genres");
            DropColumn("dbo.Genres", "Id");
            AddPrimaryKey("dbo.Genres", "GenreId");
            AddForeignKey("dbo.Movies", "GenreId", "dbo.Genres", "GenreId", cascadeDelete: true);
        }
    }
}
