﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using WfpBotConsole.Data;

namespace WfpBotConsole.Migrations
{
	[DbContext(typeof(ApplicationDbContext))]
	[Migration("20210313120153_Add Users and Chats tables")]
	partial class AddUsersandChatstables
	{
		protected override void BuildTargetModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasAnnotation("ProductVersion", "5.0.1");

			modelBuilder.Entity("ChatUser", b =>
				{
					b.Property<int>("ChatsId")
						.HasColumnType("INTEGER");

					b.Property<int>("UsersId")
						.HasColumnType("INTEGER");

					b.HasKey("ChatsId", "UsersId");

					b.HasIndex("UsersId");

					b.ToTable("ChatUser");
				});

			modelBuilder.Entity("WfpBotConsole.Models.Chat", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("INTEGER");

					b.Property<long>("ChatId")
						.HasColumnType("INTEGER");

					b.Property<string>("Title")
						.HasColumnType("TEXT");

					b.Property<int>("Type")
						.HasColumnType("INTEGER");

					b.HasKey("Id");

					b.ToTable("Chats");
				});

			modelBuilder.Entity("WfpBotConsole.Models.GameResult", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("INTEGER");

					b.Property<long>("ChatId")
						.HasColumnType("INTEGER");

					b.Property<DateTime>("PlayedAt")
						.HasColumnType("TEXT");

					b.Property<int>("UserId")
						.HasColumnType("INTEGER");

					b.Property<string>("UserName")
						.HasColumnType("TEXT");

					b.HasKey("Id");

					b.ToTable("Results");
				});

			modelBuilder.Entity("WfpBotConsole.Models.Player", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("INTEGER");

					b.Property<long>("ChatId")
						.HasColumnType("INTEGER");

					b.Property<int>("UserId")
						.HasColumnType("INTEGER");

					b.Property<string>("UserName")
						.HasColumnType("TEXT");

					b.HasKey("Id");

					b.ToTable("Players");
				});

			modelBuilder.Entity("WfpBotConsole.Models.User", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("INTEGER");

					b.Property<string>("FirstName")
						.HasColumnType("TEXT");

					b.Property<bool>("IsBot")
						.HasColumnType("INTEGER");

					b.Property<string>("LanguageCode")
						.HasColumnType("TEXT");

					b.Property<string>("LastName")
						.HasColumnType("TEXT");

					b.Property<int>("UserId")
						.HasColumnType("INTEGER");

					b.Property<string>("Username")
						.HasColumnType("TEXT");

					b.HasKey("Id");

					b.ToTable("Users");
				});

			modelBuilder.Entity("ChatUser", b =>
				{
					b.HasOne("WfpBotConsole.Models.Chat", null)
						.WithMany()
						.HasForeignKey("ChatsId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("WfpBotConsole.Models.User", null)
						.WithMany()
						.HasForeignKey("UsersId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});
#pragma warning restore 612, 618
		}
	}
}
