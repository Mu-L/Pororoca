Name:       Pororoca
Version:    3.9.1
Release:    1%{?dist}
Summary:    This is Pororoca, an HTTP inspection tool.
License:    GPLv3+
BuildArch:  x86_64
URL:        https://pororoca.io
Requires: ca-certificates
Requires: glibc
Requires: krb5-libs
Requires: libgcc
Requires: libstdc++
Requires: libicu
Requires: openssl-libs
Requires: tzdata
Requires: libICE
Requires: libSM
Requires: fontconfig
Requires: libX11

%description
This is Pororoca, an HTTP inspection tool.

%prep
# we have no source, so nothing here

%build
# building already done, so nothing here

%install
install -m 755 %{_sourcedir}/others/pororoca.sh %{buildroot}/usr/bin/pororoca
install -m 666 -D %{_sourcedir}/binaries/* -t %{buildroot}/usr/lib/pororoca/
chmod +x %{buildroot}/usr/lib/pororoca/Pororoca
install -m 666 %{_sourcedir}/others/Pororoca.desktop %{buildroot}/usr/share/applications/Pororoca.desktop
install -m 666 %{_sourcedir}/others/pororoca_icon_1024px.png %{buildroot}/usr/share/pixmaps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_logo.svg %{buildroot}/usr/share/icons/hicolor/scalable/apps/pororoca.svg
install -m 666 %{_sourcedir}/others/pororoca_icon_16px.png %{buildroot}/usr/share/icons/hicolor/16x16/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_32px.png %{buildroot}/usr/share/icons/hicolor/32x32/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_48px.png %{buildroot}/usr/share/icons/hicolor/48x48/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_64px.png %{buildroot}/usr/share/icons/hicolor/64x64/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_128px.png %{buildroot}/usr/share/icons/hicolor/128x128/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_256px.png %{buildroot}/usr/share/icons/hicolor/256x256/apps/pororoca.png
install -m 666 %{_sourcedir}/others/pororoca_icon_512px.png %{buildroot}/usr/share/icons/hicolor/512x512/apps/pororoca.png

%files
/usr/bin/pororoca
/usr/lib/pororoca/*
/usr/share/applications/Pororoca.desktop
/usr/share/pixmaps/pororoca.png
/usr/share/icons/hicolor/scalable/apps/pororoca.svg
/usr/share/icons/hicolor/16x16/apps/pororoca.png
/usr/share/icons/hicolor/32x32/apps/pororoca.png
/usr/share/icons/hicolor/48x48/apps/pororoca.png
/usr/share/icons/hicolor/64x64/apps/pororoca.png
/usr/share/icons/hicolor/128x128/apps/pororoca.png
/usr/share/icons/hicolor/256x256/apps/pororoca.png
/usr/share/icons/hicolor/512x512/apps/pororoca.png

%changelog
# let's skip this for now