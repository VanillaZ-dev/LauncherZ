<Window x:Class="LauncherZ.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:conv="clr-namespace:LauncherZ.Views"
        Title="LauncherZ"
        Width="860" Height="700"
        MinWidth="860" MinHeight="700"
        MaxWidth="860" MaxHeight="700"
        ResizeMode="CanMinimize"
        WindowStyle="None"
        AllowsTransparency="True"
        Background="Transparent"
        WindowStartupLocation="CenterScreen">
    <Window.Resources>
        <conv:PingToColorConverter x:Key="PingColor"/>
        <conv:BoolToVisibilityConverter x:Key="BoolVis"/>
        <conv:InverseBoolToVisibilityConverter x:Key="InvBoolVis"/>
        <conv:PlayerCountToColorConverter x:Key="PlayerColor"/>
        <conv:BoolToFavColorConverter x:Key="BoolToFavColor"/>
        <conv:TagBgConverter x:Key="TagBgConv"/>
        <conv:TagBdrConverter x:Key="TagBdrConv"/>
        <conv:TagTxtConverter x:Key="TagTxtConv"/>
        <conv:TagLabelConverter x:Key="TagLabelConv"/>

        <Style x:Key="TabBtn" TargetType="Button">
            <Setter Property="Background" Value="{DynamicResource TabInactBgBrush}"/>
            <Setter Property="Foreground" Value="{DynamicResource TabInactTxtBrush}"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="FontFamily" Value="{StaticResource DisplayFont}"/>
            <Setter Property="FontSize" Value="14"/>
            <Setter Property="FontWeight" Value="Bold"/>
            <Setter Property="Height" Value="50"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Grid>
                            <Border x:Name="Bg" Background="{TemplateBinding Background}"
                                    BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,1,0"
                                    Padding="16,0">
                                <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                            </Border>
                            <Border x:Name="Line" VerticalAlignment="Bottom" Height="3"
                                    Background="{DynamicResource AccentBrush}" Visibility="Collapsed"/>
                        </Grid>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="Bg" Property="Background" Value="{DynamicResource BgHoverBrush}"/>
                                <Setter Property="Foreground" Value="{DynamicResource TextPrimaryBrush}"/>
                            </Trigger>
                            <Trigger Property="Tag" Value="active">
                                <Setter TargetName="Bg" Property="Background" Value="{DynamicResource TabActiveBgBrush}"/>
                                <Setter Property="Foreground" Value="{DynamicResource TabActiveTxtBrush}"/>
                                <Setter TargetName="Line" Property="Visibility" Value="Visible"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="ChipBtn" TargetType="ToggleButton">
            <Setter Property="Background" Value="{DynamicResource BgRaisedBrush}"/>
            <Setter Property="Foreground" Value="{DynamicResource TextSecondBrush}"/>
            <Setter Property="BorderBrush" Value="{DynamicResource BorderBrush}"/>
            <Setter Property="BorderThickness" Value="1"/>
            <Setter Property="Padding" Value="11,5"/>
            <Setter Property="FontSize" Value="12"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="ToggleButton">
                        <Border Background="{TemplateBinding Background}"
                                BorderBrush="{TemplateBinding BorderBrush}"
                                BorderThickness="{TemplateBinding BorderThickness}"
                                CornerRadius="5" Padding="{TemplateBinding Padding}">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsChecked" Value="True">
                                <Setter Property="Background" Value="{DynamicResource AccentDimBrush}"/>
                                <Setter Property="BorderBrush" Value="{DynamicResource AccentBrush}"/>
                                <Setter Property="Foreground" Value="{DynamicResource AccentBrush}"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="ConnectStyle" TargetType="Button">
            <Setter Property="Background" Value="{DynamicResource ConnectBtnBrush}"/>
            <Setter Property="Foreground" Value="#f5ede6"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="FontFamily" Value="{StaticResource DisplayFont}"/>
            <Setter Property="FontSize" Value="18"/>
            <Setter Property="FontWeight" Value="Bold"/>
            <Setter Property="Height" Value="52"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border x:Name="Bd" Background="{TemplateBinding Background}"
                                CornerRadius="7" Padding="28,0">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="Bd" Property="Background" Value="{DynamicResource ConnectBtnHBrush}"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="WinBtn" TargetType="Button">
            <Setter Property="Width" Value="28"/>
            <Setter Property="Height" Value="18"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="FontSize" Value="11"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}" CornerRadius="3">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="ColHdr" TargetType="Button">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Foreground" Value="{DynamicResource TextSecondBrush}"/>
            <Setter Property="FontSize" Value="11"/>
            <Setter Property="FontWeight" Value="Bold"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}" Padding="4,0">
                            <ContentPresenter/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter Property="Foreground" Value="{DynamicResource TextPrimaryBrush}"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="ServerRow" TargetType="ListViewItem">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="Foreground" Value="{DynamicResource TextPrimaryBrush}"/>
            <Setter Property="BorderThickness" Value="0,0,0,1"/>
            <Setter Property="BorderBrush" Value="{DynamicResource BorderBrush}"/>
            <Setter Property="Padding" Value="18,0"/>
            <Setter Property="Height" Value="42"/>
            <Setter Property="HorizontalContentAlignment" Value="Stretch"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="ListViewItem">
                        <Grid>
                            <Border x:Name="Bd"
                                    Background="{TemplateBinding Background}"
                                    BorderBrush="{TemplateBinding BorderBrush}"
                                    BorderThickness="{TemplateBinding BorderThickness}"
                                    Padding="{TemplateBinding Padding}">
                                <ContentPresenter VerticalAlignment="Center"/>
                            </Border>
                            <Border x:Name="Accent" Width="3" HorizontalAlignment="Left"
                                    Background="{DynamicResource AccentBrush}" Visibility="Collapsed"/>
                        </Grid>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="Bd" Property="Background" Value="{DynamicResource BgHoverBrush}"/>
                            </Trigger>
                            <Trigger Property="IsSelected" Value="True">
                                <Setter TargetName="Bd" Property="Background" Value="{DynamicResource BgSelectedBrush}"/>
                                <Setter TargetName="Accent" Property="Visibility" Value="Visible"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </Window.Resources>

    <Border Margin="10" CornerRadius="8">
        <Border.Effect>
            <DropShadowEffect Color="Black" BlurRadius="20" ShadowDepth="0" Opacity="0.7"/>
        </Border.Effect>
        <Border Background="{DynamicResource BgRootBrush}" CornerRadius="8" ClipToBounds="True">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="38"/>
                    <RowDefinition Height="50"/>
                    <RowDefinition Height="44"/>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="34"/>
                    <RowDefinition Height="2"/>
                    <RowDefinition Height="30"/>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="28"/>
                </Grid.RowDefinitions>

                <!-- TITLEBAR -->
                <Grid Grid.Row="0" Background="{DynamicResource BgRootBrush}"
                      MouseLeftButtonDown="TitleBar_MouseDown">
                    <Border BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,1"/>
                    <DockPanel Margin="14,0">
                        <StackPanel Orientation="Horizontal" VerticalAlignment="Center" DockPanel.Dock="Left">
                            <Image Source="/Resources/logo.png" Width="26" Height="26" VerticalAlignment="Center"/>
                            <TextBlock VerticalAlignment="Center" FontFamily="{StaticResource DisplayFont}"
                                       FontSize="17" FontWeight="Bold" Margin="8,0,0,0">
                                <Run Foreground="#00f5c8">LAUNCHER</Run><Run Foreground="#ff2040">Z</Run>
                            </TextBlock>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right"
                                    DockPanel.Dock="Right" VerticalAlignment="Center">
                            <Button Style="{StaticResource WinBtn}" Background="#14ffffff"
                                    Foreground="{DynamicResource TextSecondBrush}"
                                    Content="&#x2212;" Click="Minimize_Click"/>
                            <Button Style="{StaticResource WinBtn}" Background="#14ffffff"
                                    Foreground="{DynamicResource TextSecondBrush}"
                                    Content="&#x25A1;" Click="Maximize_Click" Margin="2,0"/>
                            <Button Style="{StaticResource WinBtn}" Background="#b03030"
                                    Foreground="#f0d0d0" Content="&#x2715;" Click="Close_Click"/>
                        </StackPanel>
                    </DockPanel>
                </Grid>

                <!-- TAB ROW -->
                <Border Grid.Row="1" Background="{DynamicResource BgPanelBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,2">
                    <DockPanel>
                        <StackPanel Orientation="Horizontal" DockPanel.Dock="Left" Margin="14,0,0,0">
                            <Button x:Name="TabOfficial"  Style="{StaticResource TabBtn}" Tag="active" Content="OFFICIAL"  Click="Tab_Click"/>
                            <Button x:Name="TabCommunity" Style="{StaticResource TabBtn}" Content="COMMUNITY" Click="Tab_Click"/>
                            <Button x:Name="TabFavorites" Style="{StaticResource TabBtn}" Content="FAVORITES" Click="Tab_Click"/>
                            <Button x:Name="TabRecent"    Style="{StaticResource TabBtn}" Content="RECENT"    Click="Tab_Click"/>
                        </StackPanel>
                        <Border Width="1" Background="{DynamicResource BorderLitBrush}"
                                Margin="12,8" DockPanel.Dock="Left"/>
                        <DockPanel DockPanel.Dock="Right" Margin="0,0,14,0" LastChildFill="True">
                            <StackPanel Orientation="Horizontal" DockPanel.Dock="Right" VerticalAlignment="Center" Margin="8,0,0,0">
                                <!-- Mods button -->
                                <Button Click="Mods_Click" Cursor="Hand" Height="32" Margin="0,0,8,0"
                                        Background="{DynamicResource BgRaisedBrush}"
                                        Foreground="{DynamicResource TextSecondBrush}"
                                        BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1">
                                    <Button.Template>
                                        <ControlTemplate TargetType="Button">
                                            <Border Background="{TemplateBinding Background}"
                                                    BorderBrush="{TemplateBinding BorderBrush}"
                                                    BorderThickness="{TemplateBinding BorderThickness}"
                                                    CornerRadius="6" Padding="13,0">
                                                <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                                    <Path Data="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"
                                                          Stroke="{TemplateBinding Foreground}" StrokeThickness="1.5"
                                                          Width="14" Height="14" Stretch="Uniform" Margin="0,0,6,0"/>
                                                    <TextBlock Text="Mods" FontSize="13" FontWeight="SemiBold"
                                                               Foreground="{TemplateBinding Foreground}" VerticalAlignment="Center"/>
                                                </StackPanel>
                                            </Border>
                                            <ControlTemplate.Triggers>
                                                <Trigger Property="IsMouseOver" Value="True">
                                                    <Setter Property="BorderBrush" Value="{DynamicResource AccentBrush}"/>
                                                    <Setter Property="Foreground" Value="{DynamicResource AccentBrush}"/>
                                                </Trigger>
                                            </ControlTemplate.Triggers>
                                        </ControlTemplate>
                                    </Button.Template>
                                </Button>
                                <!-- Settings -->
                                <Button x:Name="SettingsBtn" Click="Settings_Click" Cursor="Hand"
                                        Width="34" Height="34"
                                        Background="{DynamicResource BgRaisedBrush}"
                                        Foreground="{DynamicResource TextSecondBrush}"
                                        BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1">
                                    <Button.Template>
                                        <ControlTemplate TargetType="Button">
                                            <Border Background="{TemplateBinding Background}"
                                                    BorderBrush="{TemplateBinding BorderBrush}"
                                                    BorderThickness="{TemplateBinding BorderThickness}"
                                                    CornerRadius="6">
                                                <Path Data="M12 2C10.9 2 10 2.9 10 4L10 4.1C9.4 4.3 8.9 4.6 8.4 5L8.3 4.9C7.5 4.1 6.3 4.1 5.5 4.9L4.9 5.5C4.1 6.3 4.1 7.5 4.9 8.3L5 8.4C4.6 8.9 4.3 9.4 4.1 10L4 10C2.9 10 2 10.9 2 12C2 13.1 2.9 14 4 14L4.1 14C4.3 14.6 4.6 15.1 5 15.6L4.9 15.7C4.1 16.5 4.1 17.7 4.9 18.5L5.5 19.1C6.3 19.9 7.5 19.9 8.3 19.1L8.4 19C8.9 19.4 9.4 19.7 10 19.9L10 20C10 21.1 10.9 22 12 22C13.1 22 14 21.1 14 20L14 19.9C14.6 19.7 15.1 19.4 15.6 19L15.7 19.1C16.5 19.9 17.7 19.9 18.5 19.1L19.1 18.5C19.9 17.7 19.9 16.5 19.1 15.7L19 15.6C19.4 15.1 19.7 14.6 19.9 14L20 14C21.1 14 22 13.1 22 12C22 10.9 21.1 10 20 10L19.9 10C19.7 9.4 19.4 8.9 19 8.4L19.1 8.3C19.9 7.5 19.9 6.3 19.1 5.5L18.5 4.9C17.7 4.1 16.5 4.1 15.7 4.9L15.6 5C15.1 4.6 14.6 4.3 14 4.1L14 4C14 2.9 13.1 2 12 2ZM12 9C13.7 9 15 10.3 15 12C15 13.7 13.7 15 12 15C10.3 15 9 13.7 9 12C9 10.3 10.3 9 12 9Z"
                                                      Fill="{TemplateBinding Foreground}" Width="16" Height="16"
                                                      Stretch="Uniform" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                                            </Border>
                                            <ControlTemplate.Triggers>
                                                <Trigger Property="IsMouseOver" Value="True">
                                                    <Setter Property="Foreground" Value="{DynamicResource TextPrimaryBrush}"/>
                                                </Trigger>
                                                <Trigger Property="Tag" Value="active">
                                                    <Setter Property="Background" Value="{DynamicResource AccentDimBrush}"/>
                                                    <Setter Property="BorderBrush" Value="{DynamicResource AccentBrush}"/>
                                                    <Setter Property="Foreground" Value="{DynamicResource AccentBrush}"/>
                                                </Trigger>
                                            </ControlTemplate.Triggers>
                                        </ControlTemplate>
                                    </Button.Template>
                                </Button>
                            </StackPanel>
                            <!-- Search -->
                            <Grid VerticalAlignment="Center">
                                <Border Background="{DynamicResource BgRaisedBrush}"
                                        BorderBrush="{DynamicResource BorderLitBrush}"
                                        BorderThickness="1" CornerRadius="6" Padding="30,6,11,6">
                                    <TextBox x:Name="SearchBox" Background="Transparent" BorderThickness="0"
                                             FontSize="13" Foreground="{DynamicResource TextPrimaryBrush}"
                                             CaretBrush="{DynamicResource TextPrimaryBrush}"
                                             TextChanged="Search_TextChanged"/>
                                </Border>
                                <TextBlock IsHitTestVisible="False" Text="Search name, map, IP..."
                                           Foreground="{DynamicResource TextDimBrush}"
                                           FontSize="13" Margin="32,0,0,0" VerticalAlignment="Center">
                                    <TextBlock.Style>
                                        <Style TargetType="TextBlock">
                                            <Setter Property="Visibility" Value="Collapsed"/>
                                            <Style.Triggers>
                                                <DataTrigger Binding="{Binding Text, ElementName=SearchBox}" Value="">
                                                    <Setter Property="Visibility" Value="Visible"/>
                                                </DataTrigger>
                                            </Style.Triggers>
                                        </Style>
                                    </TextBlock.Style>
                                </TextBlock>
                                <Path Data="M21 21L16.65 16.65M11 2A9 9 0 1 0 11 20A9 9 0 0 0 11 2"
                                      Stroke="{DynamicResource TextDimBrush}" StrokeThickness="2"
                                      Width="14" Height="14" Stretch="Uniform"
                                      HorizontalAlignment="Left" Margin="9,0,0,0" IsHitTestVisible="False"/>
                            </Grid>
                        </DockPanel>
                    </DockPanel>
                </Border>

                <!-- FILTER BAR -->
                <Border Grid.Row="2" Background="{DynamicResource BgPanelBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,1">
                    <DockPanel Margin="16,0">
                        <TextBlock Text="FILTER" DockPanel.Dock="Left" VerticalAlignment="Center"
                                   FontSize="11" FontWeight="Bold"
                                   Foreground="{DynamicResource TextDimBrush}" Margin="0,0,8,0"/>
                        <Border DockPanel.Dock="Left" Background="{DynamicResource BgRaisedBrush}"
                                BorderBrush="{DynamicResource BorderBrush}" BorderThickness="1"
                                CornerRadius="5" Padding="11,5" Margin="0,0,6,0">
                            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                <TextBlock Text="Map:" FontSize="12"
                                           Foreground="{DynamicResource TextSecondBrush}"
                                           VerticalAlignment="Center" Margin="0,0,4,0"/>
                                <ComboBox x:Name="MapFilter" Width="100" FontSize="12"
                                          Background="Transparent" BorderThickness="0"
                                          Foreground="{DynamicResource TextSecondBrush}"
                                          SelectionChanged="MapFilter_Changed"/>
                            </StackPanel>
                        </Border>
                        <ToggleButton DockPanel.Dock="Left" Style="{StaticResource ChipBtn}"
                                      Content="Vanilla Only" Margin="0,0,6,0"
                                      IsChecked="{Binding FilterVanillaOnly}"/>
                        <ToggleButton DockPanel.Dock="Left" Style="{StaticResource ChipBtn}"
                                      Content="First Person" Margin="0,0,6,0"
                                      IsChecked="{Binding FilterFirstPerson}"/>
                        <Border DockPanel.Dock="Left" Background="{DynamicResource BgRaisedBrush}"
                                BorderBrush="{DynamicResource BorderBrush}" BorderThickness="1"
                                CornerRadius="5" Padding="11,5" Margin="0,0,6,0">
                            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                <TextBlock Text="Ping:" FontSize="12"
                                           Foreground="{DynamicResource TextSecondBrush}"
                                           VerticalAlignment="Center" Margin="0,0,4,0"/>
                                <ComboBox x:Name="PingFilter" Width="90" FontSize="12"
                                          Background="Transparent" BorderThickness="0"
                                          Foreground="{DynamicResource TextSecondBrush}"
                                          SelectionChanged="PingFilter_Changed">
                                    <ComboBoxItem Content="Any" Tag="0"/>
                                    <ComboBoxItem Content="Under 50ms" Tag="50"/>
                                    <ComboBoxItem Content="Under 100ms" Tag="100"/>
                                    <ComboBoxItem Content="Under 150ms" Tag="150"/>
                                </ComboBox>
                            </StackPanel>
                        </Border>
                        <TextBlock DockPanel.Dock="Right" VerticalAlignment="Center"
                                   FontFamily="{StaticResource MonoFont}" FontSize="12"
                                   Foreground="{DynamicResource TextDimBrush}">
                            <Run Text="Showing "/>
                            <Run x:Name="VisibleRun" Foreground="{DynamicResource TextSecondBrush}"/>
                            <Run Text=" of "/>
                            <Run x:Name="TotalRun" Foreground="{DynamicResource TextSecondBrush}"/>
                        </TextBlock>
                    </DockPanel>
                </Border>

                <!-- INFO CARD -->
                <Border Grid.Row="3" Background="{DynamicResource BgPanelBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,2"
                        Padding="18,12,18,10">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                        </Grid.RowDefinitions>
                        <DockPanel Grid.Row="0" Margin="0,0,0,8">
                            <Button DockPanel.Dock="Right" Style="{StaticResource ConnectStyle}"
                                    x:Name="ConnectBtn" Click="Connect_Click">
                                <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                    <Path Data="M8 5L8 19L19 12Z" Fill="White" Width="16" Height="16"
                                          Stretch="Uniform" Margin="0,0,8,0"/>
                                    <TextBlock x:Name="ConnectLabel" Text="CONNECT" VerticalAlignment="Center"/>
                                </StackPanel>
                            </Button>
                            <StackPanel Orientation="Horizontal" VerticalAlignment="Center" DockPanel.Dock="Left">
                                <Border x:Name="ServerTagBadge" CornerRadius="3" Padding="7,2" Margin="0,0,8,0">
                                    <TextBlock x:Name="ServerTagText" FontSize="10" FontWeight="Bold"/>
                                </Border>
                                <StackPanel VerticalAlignment="Center">
                                    <TextBlock x:Name="ServerNameText"
                                               FontFamily="{StaticResource DisplayFont}"
                                               FontSize="19" FontWeight="Bold"
                                               Foreground="{DynamicResource TextPrimaryBrush}"
                                               TextTrimming="CharacterEllipsis"/>
                                    <TextBlock x:Name="ServerIpText"
                                               FontFamily="{StaticResource MonoFont}"
                                               FontSize="11" Foreground="{DynamicResource TextSecondBrush}"
                                               Cursor="Hand" MouseLeftButtonDown="IP_Click"/>
                                </StackPanel>
                            </StackPanel>
                        </DockPanel>
                        <StackPanel Grid.Row="1" Orientation="Horizontal">
                            <StackPanel HorizontalAlignment="Center" Margin="0,0,22,0">
                                <TextBlock x:Name="StatPlayers" Text="--/--"
                                           FontFamily="{StaticResource MonoFont}"
                                           FontSize="26" FontWeight="SemiBold"
                                           Foreground="{DynamicResource TextPrimaryBrush}"/>
                                <TextBlock Text="PLAYERS" FontSize="10" FontWeight="Bold"
                                           Foreground="{DynamicResource TextSecondBrush}"/>
                            </StackPanel>
                            <Border Width="1" Height="40" Background="{DynamicResource BorderLitBrush}" Margin="0,0,22,0"/>
                            <StackPanel HorizontalAlignment="Center" Margin="0,0,22,0">
                                <TextBlock x:Name="StatQueue" Text="--"
                                           FontFamily="{StaticResource MonoFont}"
                                           FontSize="26" FontWeight="SemiBold"
                                           Foreground="{DynamicResource QueueColorBrush}"/>
                                <TextBlock Text="QUEUE" FontSize="10" FontWeight="Bold"
                                           Foreground="{DynamicResource TextSecondBrush}"/>
                            </StackPanel>
                            <Border Width="1" Height="40" Background="{DynamicResource BorderLitBrush}" Margin="0,0,22,0"/>
                            <StackPanel HorizontalAlignment="Center" Margin="0,0,22,0">
                                <TextBlock x:Name="StatPing" Text="--"
                                           FontFamily="{StaticResource MonoFont}"
                                           FontSize="26" FontWeight="SemiBold"
                                           Foreground="{DynamicResource TextPrimaryBrush}"/>
                                <TextBlock Text="PING" FontSize="10" FontWeight="Bold"
                                           Foreground="{DynamicResource TextSecondBrush}"/>
                            </StackPanel>
                            <Border Width="1" Height="40" Background="{DynamicResource BorderLitBrush}" Margin="0,0,22,0"/>
                            <StackPanel HorizontalAlignment="Center">
                                <TextBlock x:Name="StatTime" Text="--:--"
                                           FontFamily="{StaticResource MonoFont}"
                                           FontSize="26" FontWeight="SemiBold"
                                           Foreground="{DynamicResource TextPrimaryBrush}"/>
                                <TextBlock Text="IN-GAME TIME" FontSize="10" FontWeight="Bold"
                                           Foreground="{DynamicResource TextSecondBrush}"/>
                            </StackPanel>
                        </StackPanel>
                    </Grid>
                </Border>

                <!-- LIST TOPBAR -->
                <Border Grid.Row="4" Background="{DynamicResource BgRootBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,1">
                    <DockPanel Margin="18,0">
                        <Button DockPanel.Dock="Left" Click="Refresh_Click" Cursor="Hand" Height="26">
                            <Button.Template>
                                <ControlTemplate TargetType="Button">
                                    <Border x:Name="Bd" Background="{DynamicResource AccentDimBrush}"
                                            BorderBrush="{DynamicResource AccentBrush}"
                                            BorderThickness="1" CornerRadius="5" Padding="12,0">
                                        <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                            <Path Data="M23 4L23 10L17 10M1 20L1 14L7 14M3.51 9A9 9 0 0 1 18.36 5.64L23 10M1 14L5.64 18.36A9 9 0 0 0 20.49 15"
                                                  Stroke="{DynamicResource AccentBrush}" StrokeThickness="2"
                                                  Width="13" Height="13" Stretch="Uniform" Margin="0,0,6,0"/>
                                            <TextBlock Text="Refresh Servers" FontSize="12" FontWeight="SemiBold"
                                                       Foreground="{DynamicResource AccentBrush}" VerticalAlignment="Center"/>
                                        </StackPanel>
                                    </Border>
                                </ControlTemplate>
                            </Button.Template>
                        </Button>
                        <TextBlock DockPanel.Dock="Right" VerticalAlignment="Center"
                                   FontFamily="{StaticResource MonoFont}" FontSize="11"
                                   Foreground="{DynamicResource TextDimBrush}">
                            <Run x:Name="ScanStatusRun" Foreground="{DynamicResource TextSecondBrush}"/>
                            <Run Text=" · "/>
                            <Run x:Name="FilteredRun"/>
                        </TextBlock>
                    </DockPanel>
                </Border>

                <!-- SCAN BAR -->
                <Grid Grid.Row="5" Background="{DynamicResource BorderBrush}">
                    <Rectangle x:Name="ScanBar" HorizontalAlignment="Left" Width="0"
                                Fill="{DynamicResource AccentBrush}" Height="2"/>
                </Grid>

                <!-- COLUMN HEADERS -->
                <Border Grid.Row="6" Background="{DynamicResource BgPanelBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,0,0,1">
                    <Grid Margin="18,0">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="34"/>
                            <ColumnDefinition Width="*"/>
                            <ColumnDefinition Width="110"/>
                            <ColumnDefinition Width="110"/>
                            <ColumnDefinition Width="85"/>
                            <ColumnDefinition Width="80"/>
                            <ColumnDefinition Width="70"/>
                            <ColumnDefinition Width="30"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Grid.Column="0"/>
                        <TextBlock Grid.Column="1" Text="NAME" FontSize="11" FontWeight="Bold"
                                   Foreground="{DynamicResource TextSecondBrush}"
                                   VerticalAlignment="Center" Margin="4,0"/>
                        <Button Grid.Column="2" Style="{StaticResource ColHdr}" Click="SortMap_Click">
                            <StackPanel Orientation="Horizontal">
                                <TextBlock x:Name="HdrMap" Text="MAP" FontSize="11" FontWeight="Bold" VerticalAlignment="Center"/>
                                <TextBlock x:Name="ArrMap" Text=" ↑" FontSize="10" VerticalAlignment="Center"
                                           Opacity="1" Foreground="{DynamicResource AccentBrush}"/>
                            </StackPanel>
                        </Button>
                        <Button Grid.Column="3" Style="{StaticResource ColHdr}" Click="SortPlayers_Click">
                            <StackPanel Orientation="Horizontal">
                                <TextBlock x:Name="HdrPlayers" Text="PLAYERS" FontSize="11" FontWeight="Bold" VerticalAlignment="Center"/>
                                <TextBlock x:Name="ArrPlayers" Text=" ↑" FontSize="10" VerticalAlignment="Center" Opacity="0"/>
                            </StackPanel>
                        </Button>
                        <TextBlock Grid.Column="4" Text="QUEUE" FontSize="11" FontWeight="Bold"
                                   Foreground="{DynamicResource TextSecondBrush}"
                                   VerticalAlignment="Center" Margin="4,0"/>
                        <Button Grid.Column="5" Style="{StaticResource ColHdr}" Click="SortPing_Click">
                            <StackPanel Orientation="Horizontal">
                                <TextBlock x:Name="HdrPing" Text="PING" FontSize="11" FontWeight="Bold" VerticalAlignment="Center"/>
                                <TextBlock x:Name="ArrPing" Text=" ↑" FontSize="10" VerticalAlignment="Center" Opacity="0"/>
                            </StackPanel>
                        </Button>
                        <TextBlock Grid.Column="6" Text="TIME" FontSize="11" FontWeight="Bold"
                                   Foreground="{DynamicResource TextSecondBrush}"
                                   VerticalAlignment="Center" Margin="4,0"/>
                        <TextBlock Grid.Column="7" Text="&#x1F512;" FontSize="12"
                                   Foreground="{DynamicResource TextDimBrush}" VerticalAlignment="Center"/>
                    </Grid>
                </Border>

                <!-- SERVER LIST -->
                <ListView x:Name="ServerList" Grid.Row="7"
                          Background="{DynamicResource BgRootBrush}" BorderThickness="0"
                          ItemContainerStyle="{StaticResource ServerRow}"
                          SelectionChanged="ServerList_SelectionChanged"
                          VirtualizingPanel.IsVirtualizing="True"
                          VirtualizingPanel.VirtualizationMode="Recycling"
                          ScrollViewer.HorizontalScrollBarVisibility="Disabled">
                    <ListView.ItemTemplate>
                        <DataTemplate>
                            <Grid>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="34"/>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="110"/>
                                    <ColumnDefinition Width="110"/>
                                    <ColumnDefinition Width="85"/>
                                    <ColumnDefinition Width="80"/>
                                    <ColumnDefinition Width="70"/>
                                    <ColumnDefinition Width="30"/>
                                </Grid.ColumnDefinitions>
                                <TextBlock Grid.Column="0" Text="&#x2605;" FontSize="18" TextAlignment="Center"
                                           VerticalAlignment="Center" Cursor="Hand"
                                           Foreground="{Binding IsFavorite, Converter={StaticResource BoolToFavColor}}"
                                           MouseLeftButtonDown="Fav_Click"/>
                                <StackPanel Grid.Column="1" Orientation="Horizontal" VerticalAlignment="Center">
                                    <Border CornerRadius="3" Padding="6,2" Margin="0,0,5,0"
                                            Background="{Binding Converter={StaticResource TagBgConv}}"
                                            BorderBrush="{Binding Converter={StaticResource TagBdrConv}}"
                                            BorderThickness="1">
                                        <TextBlock FontSize="9" FontWeight="Bold"
                                                   Foreground="{Binding Converter={StaticResource TagTxtConv}}"
                                                   Text="{Binding Converter={StaticResource TagLabelConv}}"/>
                                    </Border>
                                    <TextBlock Text="{Binding Name}" FontSize="13" FontWeight="Medium"
                                               Foreground="{DynamicResource TextPrimaryBrush}"
                                               TextTrimming="CharacterEllipsis" VerticalAlignment="Center"/>
                                </StackPanel>
                                <TextBlock Grid.Column="2" Text="{Binding Map}" FontSize="13"
                                           Foreground="{DynamicResource TextSecondBrush}"
                                           VerticalAlignment="Center" Margin="4,0"
                                           TextTrimming="CharacterEllipsis"/>
                                <TextBlock Grid.Column="3" VerticalAlignment="Center" Margin="4,0"
                                           FontFamily="{StaticResource MonoFont}" FontSize="13" FontWeight="Medium"
                                           Foreground="{DynamicResource TextPrimaryBrush}">
                                    <TextBlock.Text>
                                        <MultiBinding StringFormat="{}{0}/{1}">
                                            <Binding Path="Players"/>
                                            <Binding Path="MaxPlayers"/>
                                        </MultiBinding>
                                    </TextBlock.Text>
                                </TextBlock>
                                <StackPanel Grid.Column="4" Orientation="Horizontal"
                                            VerticalAlignment="Center" Margin="4,0">
                                    <TextBlock Text="&#x23F1;" FontSize="11"
                                               Foreground="{DynamicResource QueueColorBrush}"
                                               Visibility="{Binding QueueCount, Converter={StaticResource BoolVis}}"/>
                                    <TextBlock FontFamily="{StaticResource MonoFont}" FontSize="13" FontWeight="SemiBold"
                                               Foreground="{DynamicResource QueueColorBrush}"
                                               Text="{Binding QueueCount}"
                                               Visibility="{Binding QueueCount, Converter={StaticResource BoolVis}}"/>
                                    <TextBlock Text="--" FontFamily="{StaticResource MonoFont}" FontSize="12"
                                               Foreground="{DynamicResource TextDimBrush}"
                                               Visibility="{Binding QueueCount, Converter={StaticResource InvBoolVis}}"/>
                                </StackPanel>
                                <StackPanel Grid.Column="5" Orientation="Horizontal"
                                            VerticalAlignment="Center" Margin="4,0">
                                    <Ellipse Width="8" Height="8" Margin="0,0,5,0"
                                             Fill="{Binding Ping, Converter={StaticResource PingColor}}"/>
                                    <TextBlock FontFamily="{StaticResource MonoFont}" FontSize="13"
                                               Foreground="{Binding Ping, Converter={StaticResource PingColor}}"
                                               Text="{Binding Ping}"/>
                                </StackPanel>
                                <TextBlock Grid.Column="6" Text="{Binding GameTime}"
                                           FontFamily="{StaticResource MonoFont}" FontSize="12"
                                           Foreground="{DynamicResource TextSecondBrush}"
                                           VerticalAlignment="Center" Margin="4,0"/>
                                <TextBlock Grid.Column="7" Text="&#x1F512;" FontSize="12"
                                           Foreground="{DynamicResource TextDimBrush}"
                                           VerticalAlignment="Center" TextAlignment="Center"
                                           Visibility="{Binding IsPasswordProtected, Converter={StaticResource BoolVis}}"/>
                            </Grid>
                        </DataTemplate>
                    </ListView.ItemTemplate>
                </ListView>

                <!-- BOTTOM BAR -->
                <Border Grid.Row="8" Background="{DynamicResource BgRootBrush}"
                        BorderBrush="{DynamicResource BorderBrush}" BorderThickness="0,1,0,0">
                    <DockPanel Margin="18,0">
                        <Ellipse Width="7" Height="7" DockPanel.Dock="Left"
                                 Fill="{DynamicResource AccentBrush}"
                                 Margin="0,0,10,0" VerticalAlignment="Center"/>
                        <TextBlock x:Name="StatusText" DockPanel.Dock="Left" VerticalAlignment="Center"
                                   FontSize="11" Foreground="{DynamicResource TextDimBrush}"/>
                        <TextBlock x:Name="VersionText" DockPanel.Dock="Right" VerticalAlignment="Center"
                                   FontFamily="{StaticResource MonoFont}" FontSize="10"
                                   Foreground="{DynamicResource TextDimBrush}"/>
                    </DockPanel>
                </Border>

                <!-- SETTINGS PANEL -->
                <Border x:Name="SettingsPanel" Grid.Row="0" Grid.RowSpan="9"
                        Background="{DynamicResource BgPanelBrush}"
                        BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1,0,0,0"
                        Width="380" HorizontalAlignment="Right" Visibility="Collapsed">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="*"/>
                        </Grid.RowDefinitions>
                        <Border Grid.Row="0" BorderBrush="{DynamicResource BorderBrush}"
                                BorderThickness="0,0,0,1" Padding="20,16">
                            <DockPanel>
                                <TextBlock DockPanel.Dock="Left" Text="Settings"
                                           FontFamily="{StaticResource DisplayFont}"
                                           FontSize="17" FontWeight="Bold"
                                           Foreground="{DynamicResource TextPrimaryBrush}"
                                           VerticalAlignment="Center"/>
                                <Button DockPanel.Dock="Right" Content="&#x2715;"
                                        Click="CloseSettings_Click"
                                        Width="28" Height="28" Cursor="Hand"
                                        Background="Transparent" BorderThickness="0"
                                        Foreground="{DynamicResource TextDimBrush}" FontSize="14"/>
                            </DockPanel>
                        </Border>
                        <ScrollViewer Grid.Row="1" VerticalScrollBarVisibility="Auto">
                            <StackPanel Margin="20,16">
                                <TextBlock Text="THEME" FontSize="11" FontWeight="Bold"
                                           Foreground="{DynamicResource TextDimBrush}" Margin="0,0,0,8"/>
                                <UniformGrid x:Name="ThemeGrid" Columns="3" Rows="3"/>
                                <TextBlock Text="PLAYER" FontSize="11" FontWeight="Bold"
                                           Foreground="{DynamicResource TextDimBrush}" Margin="0,16,0,6"/>
                                <TextBlock Text="In-game name" FontSize="11"
                                           Foreground="{DynamicResource TextSecondBrush}" Margin="0,0,0,6"/>
                                <TextBox x:Name="PlayerNameBox"
                                         Background="{DynamicResource BgRaisedBrush}"
                                         Foreground="{DynamicResource TextPrimaryBrush}"
                                         BorderBrush="{DynamicResource BorderLitBrush}"
                                         BorderThickness="1" FontSize="13" Padding="12,8"
                                         TextChanged="PlayerName_Changed"/>
                                <TextBlock Text="GAME" FontSize="11" FontWeight="Bold"
                                           Foreground="{DynamicResource TextDimBrush}" Margin="0,16,0,6"/>
                                <TextBlock Text="DayZ install path" FontSize="11"
                                           Foreground="{DynamicResource TextSecondBrush}" Margin="0,0,0,6"/>
                                <DockPanel>
                                    <Button DockPanel.Dock="Right" Content="Browse"
                                            Click="BrowsePath_Click" Cursor="Hand"
                                            Margin="6,0,0,0" Padding="10,7"
                                            Background="{DynamicResource BgRaisedBrush}"
                                            Foreground="{DynamicResource TextSecondBrush}"
                                            BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1">
                                        <Button.Template>
                                            <ControlTemplate TargetType="Button">
                                                <Border Background="{TemplateBinding Background}"
                                                        BorderBrush="{TemplateBinding BorderBrush}"
                                                        BorderThickness="{TemplateBinding BorderThickness}"
                                                        CornerRadius="5" Padding="{TemplateBinding Padding}">
                                                    <ContentPresenter/>
                                                </Border>
                                            </ControlTemplate>
                                        </Button.Template>
                                    </Button>
                                    <TextBox x:Name="PathBox"
                                             Background="{DynamicResource BgRaisedBrush}"
                                             Foreground="{DynamicResource TextPrimaryBrush}"
                                             BorderBrush="{DynamicResource BorderLitBrush}"
                                             BorderThickness="1" FontSize="13" Padding="12,8"/>
                                </DockPanel>
                                <TextBlock Text="BEHAVIOUR" FontSize="11" FontWeight="Bold"
                                           Foreground="{DynamicResource TextDimBrush}" Margin="0,16,0,10"/>
                                <DockPanel Margin="0,0,0,10">
                                    <StackPanel DockPanel.Dock="Left">
                                        <TextBlock Text="Close launcher on connect" FontSize="13"
                                                   Foreground="{DynamicResource TextPrimaryBrush}"/>
                                        <TextBlock Text="Minimises after launching DayZ" FontSize="11"
                                                   Foreground="{DynamicResource TextDimBrush}"/>
                                    </StackPanel>
                                    <ToggleButton x:Name="CloseOnConnectToggle" DockPanel.Dock="Right"
                                                  Width="40" Height="22" Click="Toggle_Click"/>
                                </DockPanel>
                                <DockPanel Margin="0,0,0,10">
                                    <TextBlock DockPanel.Dock="Left" Text="Auto-refresh every 3 minutes"
                                               FontSize="13" Foreground="{DynamicResource TextPrimaryBrush}"/>
                                    <ToggleButton x:Name="AutoRefreshToggle" DockPanel.Dock="Right"
                                                  Width="40" Height="22" Click="Toggle_Click"/>
                                </DockPanel>
                                <DockPanel>
                                    <TextBlock DockPanel.Dock="Left" Text="Show empty servers"
                                               FontSize="13" Foreground="{DynamicResource TextPrimaryBrush}"/>
                                    <ToggleButton x:Name="ShowEmptyToggle" DockPanel.Dock="Right"
                                                  Width="40" Height="22" Click="Toggle_Click"/>
                                </DockPanel>
                            </StackPanel>
                        </ScrollViewer>
                    </Grid>
                </Border>

                <!-- MODS POPUP -->
                <Grid x:Name="ModsOverlay" Grid.Row="0" Grid.RowSpan="9"
                      Background="#88000000" Visibility="Collapsed">
                    <Border Background="{DynamicResource BgPanelBrush}"
                            BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1"
                            CornerRadius="10" Width="400" MaxHeight="500"
                            HorizontalAlignment="Center" VerticalAlignment="Center">
                        <Grid>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="*"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <Border Grid.Row="0" BorderBrush="{DynamicResource BorderBrush}"
                                    BorderThickness="0,0,0,1" Padding="22,16">
                                <DockPanel>
                                    <TextBlock x:Name="ModsPopupTitle" DockPanel.Dock="Left"
                                               FontFamily="{StaticResource DisplayFont}"
                                               FontSize="16" FontWeight="Bold"
                                               Foreground="{DynamicResource TextPrimaryBrush}"
                                               VerticalAlignment="Center"/>
                                    <TextBlock x:Name="ModsPopupSub" DockPanel.Dock="Left"
                                               Margin="12,0,0,0" FontSize="12"
                                               Foreground="{DynamicResource TextSecondBrush}"
                                               VerticalAlignment="Center"/>
                                </DockPanel>
                            </Border>
                            <ScrollViewer Grid.Row="1">
                                <StackPanel x:Name="ModsList" Margin="22,12,22,16"/>
                            </ScrollViewer>
                            <Border Grid.Row="2" BorderBrush="{DynamicResource BorderBrush}"
                                    BorderThickness="0,1,0,0" Padding="22,12">
                                <Button HorizontalAlignment="Right" Content="Close"
                                        Click="CloseModsPopup_Click" Cursor="Hand" Padding="18,7"
                                        Background="Transparent"
                                        BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1"
                                        Foreground="{DynamicResource TextSecondBrush}">
                                    <Button.Template>
                                        <ControlTemplate TargetType="Button">
                                            <Border Background="{TemplateBinding Background}"
                                                    BorderBrush="{TemplateBinding BorderBrush}"
                                                    BorderThickness="{TemplateBinding BorderThickness}"
                                                    CornerRadius="5" Padding="{TemplateBinding Padding}">
                                                <ContentPresenter/>
                                            </Border>
                                        </ControlTemplate>
                                    </Button.Template>
                                </Button>
                            </Border>
                        </Grid>
                    </Border>
                </Grid>

                <!-- DOWNLOAD OVERLAY -->
                <Grid x:Name="DownloadOverlay" Grid.Row="0" Grid.RowSpan="9"
                      Background="#AA000000" Visibility="Collapsed">
                    <Border Background="{DynamicResource BgPanelBrush}"
                            BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1"
                            CornerRadius="10" Width="440"
                            HorizontalAlignment="Center" VerticalAlignment="Center">
                        <Grid>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="*"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <StackPanel Grid.Row="0" Margin="26,22,26,16">
                                <TextBlock Text="Mods Required"
                                           FontFamily="{StaticResource DisplayFont}"
                                           FontSize="17" FontWeight="Bold"
                                           Foreground="{DynamicResource TextPrimaryBrush}"/>
                                <TextBlock x:Name="DlSubText" FontSize="13" Margin="0,4,0,0"
                                           Foreground="{DynamicResource TextSecondBrush}"/>
                            </StackPanel>
                            <ScrollViewer Grid.Row="1" MaxHeight="300">
                                <StackPanel x:Name="DlModsList" Margin="26,0,26,16"/>
                            </ScrollViewer>
                            <Border Grid.Row="2" BorderBrush="{DynamicResource BorderBrush}"
                                    BorderThickness="0,1,0,0" Padding="26,14">
                                <DockPanel>
                                    <Button DockPanel.Dock="Right" x:Name="DlStartBtn"
                                            Content="Download &amp; Connect"
                                            Click="DlStart_Click" Cursor="Hand"
                                            Background="{DynamicResource ConnectBtnBrush}"
                                            Foreground="#f5ede6" BorderThickness="0" Padding="20,8"
                                            FontFamily="{StaticResource DisplayFont}"
                                            FontSize="14" FontWeight="Bold">
                                        <Button.Template>
                                            <ControlTemplate TargetType="Button">
                                                <Border Background="{TemplateBinding Background}"
                                                        CornerRadius="5" Padding="{TemplateBinding Padding}">
                                                    <ContentPresenter HorizontalAlignment="Center"/>
                                                </Border>
                                            </ControlTemplate>
                                        </Button.Template>
                                    </Button>
                                    <Button DockPanel.Dock="Right" Content="Cancel"
                                            Click="DlCancel_Click" Cursor="Hand"
                                            Margin="0,0,8,0" Padding="16,8"
                                            Background="Transparent"
                                            BorderBrush="{DynamicResource BorderLitBrush}" BorderThickness="1"
                                            Foreground="{DynamicResource TextSecondBrush}">
                                        <Button.Template>
                                            <ControlTemplate TargetType="Button">
                                                <Border Background="{TemplateBinding Background}"
                                                        BorderBrush="{TemplateBinding BorderBrush}"
                                                        BorderThickness="{TemplateBinding BorderThickness}"
                                                        CornerRadius="5" Padding="{TemplateBinding Padding}">
                                                    <ContentPresenter HorizontalAlignment="Center"/>
                                                </Border>
                                            </ControlTemplate>
                                        </Button.Template>
                                    </Button>
                                </DockPanel>
                            </Border>
                        </Grid>
                    </Border>
                </Grid>

            </Grid>
        </Border>
    </Border>
</Window>
