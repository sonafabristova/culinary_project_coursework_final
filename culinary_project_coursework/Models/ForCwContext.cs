using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace culinary_project_coursework.Models;

public partial class ForCwContext : DbContext
{
    public ForCwContext()
    {
    }

    public ForCwContext(DbContextOptions<ForCwContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ДниМеню> ДниМенюs { get; set; }

    public virtual DbSet<ЕдиницаИзмерения> ЕдиницаИзмеренияs { get; set; }

    public virtual DbSet<Ингредиенты> Ингредиентыs { get; set; }

    public virtual DbSet<ЛюдиВМеню> ЛюдиВМенюs { get; set; }

    public virtual DbSet<Меню> Менюs { get; set; }

    public virtual DbSet<Пользователи> Пользователиs { get; set; }

    public virtual DbSet<ПриемыПищи> ПриемыПищиs { get; set; }

    public virtual DbSet<Рецепты> Рецептыs { get; set; }

    public virtual DbSet<СоставБлюда> СоставБлюдаs { get; set; }

    public virtual DbSet<СпискиПокупок> СпискиПокупокs { get; set; }

    public virtual DbSet<ШагиПриготовления> ШагиПриготовленияs { get; set; }

    public virtual DbSet<ЭлементыСпискаПокупок> ЭлементыСпискаПокупокs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-FETHMEHK;Initial Catalog=for_cw;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_100_CI_AS");

        modelBuilder.Entity<ДниМеню>(entity =>
        {
            entity.HasKey(e => e.IdДня).HasName("PK__Дни_меню__648E240CDAA9D672");

            entity.ToTable("Дни_меню");

            entity.Property(e => e.IdДня).HasColumnName("id_дня");
            entity.Property(e => e.FkМеню).HasColumnName("FK_меню");
            entity.Property(e => e.НазваниеДня)
                .HasMaxLength(100)
                .HasColumnName("Название_дня");
            entity.Property(e => e.НомерДня).HasColumnName("Номер_дня");

            entity.HasOne(d => d.FkМенюNavigation).WithMany(p => p.ДниМенюs)
                .HasForeignKey(d => d.FkМеню)
                .HasConstraintName("FK__Дни_меню__FK_мен__693CA210");
        });

        modelBuilder.Entity<ЕдиницаИзмерения>(entity =>
        {
            entity.HasKey(e => e.IdЕдиницыИзмерения).HasName("PK__Единица___6B22633B984F4FA0");

            entity.ToTable("Единица_измерения");

            entity.Property(e => e.IdЕдиницыИзмерения).HasColumnName("id_единицы_измерения");
            entity.Property(e => e.Название).HasMaxLength(20);
        });

        modelBuilder.Entity<Ингредиенты>(entity =>
        {
            entity.HasKey(e => e.IdИнгредиента).HasName("PK__Ингредие__53BA254FE9F3B7B9");

            entity.ToTable("Ингредиенты");

            entity.Property(e => e.IdИнгредиента).HasColumnName("id_ингредиента");
            entity.Property(e => e.FkЕдиницыИзмерения).HasColumnName("FK_единицы_измерения");
            entity.Property(e => e.Название).HasMaxLength(100);
            entity.Property(e => e.СрСтоимость)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Ср_стоимость");

            entity.HasOne(d => d.FkЕдиницыИзмеренияNavigation).WithMany(p => p.Ингредиентыs)
                .HasForeignKey(d => d.FkЕдиницыИзмерения)
                .HasConstraintName("FK__Ингредиен__FK_ед__4F7CD00D");
        });

        modelBuilder.Entity<ЛюдиВМеню>(entity =>
        {
            entity.HasKey(e => e.IdЧеловека).HasName("PK__Люди_в_м__033387093EF58A1F");

            entity.ToTable("Люди_в_меню");

            entity.Property(e => e.IdЧеловека).HasColumnName("id_человека");
            entity.Property(e => e.FkМеню).HasColumnName("FK_меню");
            entity.Property(e => e.Идентификатор).HasMaxLength(50);
            entity.Property(e => e.Название).HasMaxLength(100);

            entity.HasOne(d => d.FkМенюNavigation).WithMany(p => p.ЛюдиВМенюs)
                .HasForeignKey(d => d.FkМеню)
                .HasConstraintName("FK__Люди_в_ме__FK_ме__6C190EBB");
        });

        modelBuilder.Entity<Меню>(entity =>
        {
            entity.HasKey(e => e.IdМеню).HasName("PK__Меню__E94DDB6191D757BE");

            entity.ToTable("Меню");

            entity.Property(e => e.IdМеню).HasColumnName("id_меню");
            entity.Property(e => e.FkПользователя).HasColumnName("FK_пользователя");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.КоличествоДней).HasColumnName("Количество_дней");
            entity.Property(e => e.КоличествоЧеловек).HasColumnName("Количество_человек");
            entity.Property(e => e.Название).HasMaxLength(200);

            entity.HasOne(d => d.FkПользователяNavigation).WithMany(p => p.Менюs)
                .HasForeignKey(d => d.FkПользователя)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Меню__FK_пользов__66603565");
        });

        modelBuilder.Entity<Пользователи>(entity =>
        {
            entity.HasKey(e => e.IdПользователя).HasName("PK__Пользова__AF763DA7EFB1C766");

            entity.ToTable("Пользователи");

            entity.Property(e => e.IdПользователя).HasColumnName("id_пользователя");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Имя).HasMaxLength(100);
            entity.Property(e => e.Логин).HasMaxLength(50);
            entity.Property(e => e.Пароль).HasMaxLength(255);
            entity.Property(e => e.Почта).HasMaxLength(100);
            entity.Property(e => e.Телефон).HasMaxLength(20);
        });

        modelBuilder.Entity<ПриемыПищи>(entity =>
        {
            entity.HasKey(e => e.IdПриема).HasName("PK__Приемы_п__9D2BEDB95EC35FB0");

            entity.ToTable("Приемы_пищи");

            entity.Property(e => e.IdПриема).HasColumnName("id_приема");
            entity.Property(e => e.FkДня).HasColumnName("FK_дня");
            entity.Property(e => e.FkРецепта).HasColumnName("FK_рецепта");
            entity.Property(e => e.FkЧеловека).HasColumnName("FK_человека");
            entity.Property(e => e.Порции).HasDefaultValue(1);
            entity.Property(e => e.ТипПриема)
                .HasMaxLength(20)
                .HasColumnName("Тип_приема");

            entity.HasOne(d => d.FkДняNavigation).WithMany(p => p.ПриемыПищиs)
                .HasForeignKey(d => d.FkДня)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Приемы_пи__FK_дн__70DDC3D8");

            entity.HasOne(d => d.FkРецептаNavigation).WithMany(p => p.ПриемыПищиs)
                .HasForeignKey(d => d.FkРецепта)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Приемы_пи__FK_ре__71D1E811");

            entity.HasOne(d => d.FkЧеловекаNavigation).WithMany(p => p.ПриемыПищиs)
                .HasForeignKey(d => d.FkЧеловека)
                .HasConstraintName("FK__Приемы_пи__FK_че__6FE99F9F");
        });

        modelBuilder.Entity<Рецепты>(entity =>
        {
            entity.HasKey(e => e.IdРецепта).HasName("PK__Рецепты__D9EF2C8796492D58");

            entity.ToTable("Рецепты");

            entity.Property(e => e.IdРецепта).HasColumnName("id_рецепта");
            entity.Property(e => e.IsSystemRecipe).HasDefaultValue(false);
            entity.Property(e => e.Белки).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ВремяПриготовления).HasColumnName("Время_приготовления");
            entity.Property(e => e.Жиры).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Изображение).HasMaxLength(500);
            entity.Property(e => e.Название).HasMaxLength(200);
            entity.Property(e => e.Описание).HasMaxLength(1000);
            entity.Property(e => e.Углеводы).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Рецептыs)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK__Рецепты__Created__534D60F1");
        });

        modelBuilder.Entity<СоставБлюда>(entity =>
        {
            entity.HasKey(e => e.IdСостава).HasName("PK__Состав_б__43F5EBD7FF9F8684");

            entity.ToTable("Состав_блюда");

            entity.Property(e => e.IdСостава).HasColumnName("id_состава");
            entity.Property(e => e.FkИнгредиента).HasColumnName("FK_ингредиента");
            entity.Property(e => e.FkРецепта).HasColumnName("FK_рецепта");
            entity.Property(e => e.Количество).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.FkИнгредиентаNavigation).WithMany(p => p.СоставБлюдаs)
                .HasForeignKey(d => d.FkИнгредиента)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Состав_бл__FK_ин__59FA5E80");

            entity.HasOne(d => d.FkРецептаNavigation).WithMany(p => p.СоставБлюдаs)
                .HasForeignKey(d => d.FkРецепта)
                .HasConstraintName("FK__Состав_бл__FK_ре__59063A47");
        });

        modelBuilder.Entity<СпискиПокупок>(entity =>
        {
            entity.HasKey(e => e.IdСписка).HasName("PK__Списки_п__40602DD99624A2A5");

            entity.ToTable("Списки_покупок");

            entity.Property(e => e.IdСписка).HasColumnName("id_списка");
            entity.Property(e => e.FkПользователя).HasColumnName("FK_пользователя");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.Название).HasMaxLength(200);

            entity.HasOne(d => d.FkПользователяNavigation).WithMany(p => p.СпискиПокупокs)
                .HasForeignKey(d => d.FkПользователя)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Списки_по__FK_по__5DCAEF64");
        });

        modelBuilder.Entity<ШагиПриготовления>(entity =>
        {
            entity.HasKey(e => e.IdШага).HasName("PK__Шаги_при__4EC6B00532EBE168");

            entity.ToTable("Шаги_приготовления");

            entity.Property(e => e.IdШага).HasColumnName("id_шага");
            entity.Property(e => e.FkРецепта).HasColumnName("FK_рецепта");
            entity.Property(e => e.Изображение).HasMaxLength(500);
            entity.Property(e => e.НомерШага).HasColumnName("Номер_шага");
            entity.Property(e => e.Описание).HasMaxLength(1000);

            entity.HasOne(d => d.FkРецептаNavigation).WithMany(p => p.ШагиПриготовленияs)
                .HasForeignKey(d => d.FkРецепта)
                .HasConstraintName("FK__Шаги_приг__FK_ре__5629CD9C");
        });

        modelBuilder.Entity<ЭлементыСпискаПокупок>(entity =>
        {
            entity.HasKey(e => e.IdЭлемента).HasName("PK__Элементы__FA698E353CF3FF7D");

            entity.ToTable("Элементы_списка_покупок");

            entity.Property(e => e.IdЭлемента).HasColumnName("id_элемента");
            entity.Property(e => e.FkИнгредиента).HasColumnName("FK_ингредиента");
            entity.Property(e => e.FkСписка).HasColumnName("FK_списка");
            entity.Property(e => e.IsPurchased).HasDefaultValue(false);
            entity.Property(e => e.Количество).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.FkИнгредиентаNavigation).WithMany(p => p.ЭлементыСпискаПокупокs)
                .HasForeignKey(d => d.FkИнгредиента)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Элементы___FK_ин__628FA481");

            entity.HasOne(d => d.FkСпискаNavigation).WithMany(p => p.ЭлементыСпискаПокупокs)
                .HasForeignKey(d => d.FkСписка)
                .HasConstraintName("FK__Элементы___FK_сп__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
