using Microsoft.EntityFrameworkCore.Migrations;
using PublicTransport.Data.Constants.Enumerations;

#nullable disable

namespace PublicTransport.Data.Migrations
{
    public partial class InsertDataIntoTransportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i < 50; i++)
            {
                migrationBuilder.InsertData(
                table: "Transports",
                columns: new[] { "Number", "TransportType", },
                values: new object[] { i.ToString(), (int)TransportType.Bus, });
            }

            for (int i = 1; i < 40; i++)
            {
                migrationBuilder.InsertData(
                table: "Transports",
                columns: new[] { "Number", "TransportType", },
                values: new object[] { i.ToString(), (int)TransportType.Trolleybus, });
            }

            for (int i = 1; i < 28; i++)
            {
                migrationBuilder.InsertData(
                table: "Transports",
                columns: new[] { "Number", "TransportType", },
                values: new object[] { i.ToString(), (int)TransportType.Tram, });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
