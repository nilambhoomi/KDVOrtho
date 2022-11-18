<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="GetMAProviders.aspx.cs" Inherits="GetMAProviders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PainTrax - Intakesheet</title>
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<meta charset="utf-8" />

    <!-- start: Mobile Specific -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- end: Mobile Specific -->

    <!-- start: CSS -->
<%--    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet">--%>
<%--    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet">--%>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,300,600,700,800&subset=latin,cyrillic-ext,latin-ext' rel='stylesheet' type='text/css'>
    <!-- end: CSS -->
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <%--<script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>--%>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- --%>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <!-- bootstrap & fontawesome -->
		<link rel="stylesheet" href="../assets/css/bootstrap.css" />
		<link rel="stylesheet" href="../assets/css/font-awesome.css" />

		<!-- page specific plugin styles -->
		<link rel="stylesheet" href="../assets/css/jquery-ui.custom.css" />

		<!-- text fonts -->
		<link rel="stylesheet" href="../assets/css/ace-fonts.css" />

		<!-- ace styles -->
		<link rel="stylesheet" href="../assets/css/ace.css" class="ace-main-stylesheet" />

		<!-- ace settings handler -->
		<script src="../assets/js/ace-extra.js"></script>

    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="../assets/js/bootstrap.js"></script>

		<!-- page specific plugin scripts -->
		<script src="../assets/js/jquery-ui.custom.js"></script>
		<script src="../assets/js/jquery.ui.touch-punch.js"></script>

		<!-- ace scripts -->
		<script src="../assets/js/ace/elements.scroller.js"></script>
		<script src="../assets/js/ace/elements.colorpicker.js"></script>
		<script src="../assets/js/ace/elements.fileinput.js"></script>
		<script src="../assets/js/ace/elements.typeahead.js"></script>
		<script src="../assets/js/ace/elements.wysiwyg.js"></script>
		<script src="../assets/js/ace/elements.spinner.js"></script>
		<script src="../assets/js/ace/elements.treeview.js"></script>
		<script src="../assets/js/ace/elements.wizard.js"></script>
		<script src="../assets/js/ace/elements.aside.js"></script>
		<script src="../assets/js/ace/ace.js"></script>
		<script src="../assets/js/ace/ace.ajax-content.js"></script>
		<script src="../assets/js/ace/ace.touch-drag.js"></script>
		<script src="../assets/js/ace/ace.sidebar.js"></script>
		<script src="../assets/js/ace/ace.sidebar-scroll-1.js"></script>
		<script src="../assets/js/ace/ace.submenu-hover.js"></script>
		<script src="../assets/js/ace/ace.widget-box.js"></script>
		<script src="../assets/js/ace/ace.settings.js"></script>
		<script src="../assets/js/ace/ace.settings-rtl.js"></script>
		<script src="../assets/js/ace/ace.settings-skin.js"></script>
		<script src="../assets/js/ace/ace.widget-on-reload.js"></script>
		<script src="../assets/js/ace/ace.searchbox-autocomplete.js"></script>

			<script type="text/javascript">
			    jQuery(function ($) {

			        $('#simple-colorpicker-1').ace_colorpicker({ pull_right: true }).on('change', function () {
			            var color_class = $(this).find('option:selected').data('class');
			            var new_class = 'widget-box';
			            if (color_class != 'default') new_class += ' widget-color-' + color_class;
			            $(this).closest('.widget-box').attr('class', new_class);
			        });


			        // scrollables
			        $('.scrollable').each(function () {
			            var $this = $(this);
			            $(this).ace_scroll({
			                size: $this.attr('data-size') || 100,
			                //styleClass: 'scroll-left scroll-margin scroll-thin scroll-dark scroll-light no-track scroll-visible'
			            });
			        });
			        $('.scrollable-horizontal').each(function () {
			            var $this = $(this);
			            $(this).ace_scroll(
                          {
                              horizontal: true,
                              styleClass: 'scroll-top',//show the scrollbars on top(default is bottom)
                              size: $this.attr('data-size') || 500,
                              mouseWheelLock: true
                          }
                        ).css({ 'padding-top': 12 });
			        });

			        $(window).on('resize.scroll_reset', function () {
			            $('.scrollable-horizontal').ace_scroll('reset');
			        });


			        $('#id-checkbox-vertical').prop('checked', false).on('click', function () {
			            $('#widget-toolbox-1').toggleClass('toolbox-vertical')
                        .find('.btn-group').toggleClass('btn-group-vertical')
                        .filter(':first').toggleClass('hidden')
                        .parent().toggleClass('btn-toolbar')
			        });

			        /**
                    //or use slimScroll plugin
                    $('.slim-scrollable').each(function () {
                        var $this = $(this);
                        $this.slimScroll({
                            height: $this.data('height') || 100,
                            railVisible:true
                        });
                    });
                    */


			        /**$('.widget-box').on('setting.ace.widget' , function(e) {
                        e.preventDefault();
                    });*/

			        /**
                    $('.widget-box').on('show.ace.widget', function(e) {
                        //e.preventDefault();
                        //this = the widget-box
                    });
                    $('.widget-box').on('reload.ace.widget', function(e) {
                        //this = the widget-box
                    });
                    */

			        //$('#my-widget-box').widget_box('hide');



			        // widget boxes
			        // widget box drag & drop example
			        $('.widget-container-col').sortable({
			            connectWith: '.widget-container-col',
			            items: '> .widget-box',
			            handle: ace.vars['touch'] ? '.widget-header' : false,
			            cancel: '.fullscreen',
			            opacity: 0.8,
			            revert: true,
			            forceHelperSize: true,
			            placeholder: 'widget-placeholder',
			            forcePlaceholderSize: true,
			            tolerance: 'pointer',
			            start: function (event, ui) {
			                //when an element is moved, it's parent becomes empty with almost zero height.
			                //we set a min-height for it to be large enough so that later we can easily drop elements back onto it
			                ui.item.parent().css({ 'min-height': ui.item.height() })
			                //ui.sender.css({'min-height':ui.item.height() , 'background-color' : '#F5F5F5'})
			            },
			            update: function (event, ui) {
			                ui.item.parent({ 'min-height': '' })
			                //p.style.removeProperty('background-color');
			            }
			        });



			    });
		</script>

</head>
<body>
   
    
        	<div id="navbar" class="navbar navbar-default">
			<script type="text/javascript">
				try{ace.settings.check('navbar' , 'fixed')}catch(e){}
			</script>

			<div class="navbar-container" id="navbar-container">
				<!-- #section:basics/sidebar.mobile.toggle -->
				<button type="button" class="navbar-toggle menu-toggler pull-left" id="menu-toggler" data-target="#sidebar">
					<span class="sr-only">Toggle sidebar</span>

					<span class="icon-bar"></span>

					<span class="icon-bar"></span>

					<span class="icon-bar"></span>
				</button>

				<!-- /section:basics/sidebar.mobile.toggle -->
				<div class="navbar-header pull-left">
					<!-- #section:basics/navbar.layout.brand -->
					<a href="#" class="navbar-brand">
						<small>
							PainTrax
						</small>
					</a>

					<!-- /section:basics/navbar.layout.brand -->

					<!-- #section:basics/navbar.toggle -->

					<!-- /section:basics/navbar.toggle -->
				</div>

				<!-- #section:basics/navbar.dropdown -->
				<div class="navbar-buttons navbar-header pull-right" role="navigation">
					<ul class="nav ace-nav">
						

						<!-- #section:basics/navbar.user_menu -->
						<li class="light-blue">
							<a href="Logout.aspx">
										<i class="ace-icon fa fa-power-off"></i>
										Logout
									</a>

						</li>

						<!-- /section:basics/navbar.user_menu -->
					</ul>
				</div>

				<!-- /section:basics/navbar.dropdown -->
			</div><!-- /.navbar-container -->
		</div>
        <!-- start: Header -->
    	<div class="main-container" id="main-container">
        <div class="main-content">
           <div class="main-content-inner">
                <div class="page-content">
                     <form id="form2" runat="server">
                             <asp:ScriptManager runat="server" ID="scpmgr"></asp:ScriptManager>
                    <div class="page-header">
							<h1>
                                <small>
								Welcome								
									<i class="ace-icon fa fa-angle-double-right"></i>
								</small>
							</h1>
						</div>
                    	<div class="row">
							<div class="col-xs-12">
											<div class="widget-box">
												<div class="widget-header">
													<h4 class="widget-title"></h4>
                                                    </div>
                                                	<div class="widget-body">
													<div class="widget-main">
														<div class="form-group row">
                                                           <div class="col-sm-3">
                                                            <label class="control-label no-padding-top" for="ddLocation">Location</label>
                                                               </div>
                                                            <div class="col-sm-3">
                                                             <asp:DropDownList ID="ddLocation" runat="server" CssClass="form-control">
                                           
                                        </asp:DropDownList>
                                                                 </div>
                                                            </div>
                                                        <div class="form-group row">
                                                            <div class="col-sm-3">
                                                             <label class="control-label no-padding-top" for="lbMAandProviders">Provider/MA</label>
                                                            </div>
                                                                 <div class="col-sm-3">
                                                            <asp:ListBox ID="lbMAandProviders" Rows="10" SelectionMode="Multiple" CssClass="form-control" runat="server"></asp:ListBox>
                                                        </div>
                                                               <div class="col-sm-2">       
                                                                    <asp:Button ID="moveAllLeft" CssClass="btn btn-default" runat="server" Text="<<" OnClick="moveAllLeft_Click" Visible="False" />
                                            
                                                    <asp:Button ID="moveLeft" CssClass="btn btn-default" runat="server" Text="<" OnClick="moveLeft_Click" />
                                            
                                                    <asp:Button ID="moveRight" CssClass="btn btn-default" runat="server" Text=">" OnClick="moveRight_Click" />
                                           
                                                    <asp:Button ID="moveAllRight" CssClass="btn btn-default" runat="server" Text=">>" OnClick="moveAllRight_Click" Visible="False" />
                                            </div>
                                                         <div class="col-sm-3">     <asp:ListBox CssClass="form-control" ID="lbSelectedMAandProviders" SelectionMode="Multiple" runat="server"></asp:ListBox>
                                                       </div>
                                                             
                                                             </div>
                                                        <div class="form-group">
                                                              <asp:Button ID="btnProceed" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnProceed_Click" />
                                                        </div>
                                                        </div>
                                                        </div>
                                                </div>
                           
                        </div>
                    </div>
               </form>
                           </div>
              
           </div>
                </div>
            	<div class="footer">
				<div class="footer-inner">
					<!-- #section:basics/footer -->
					<div class="footer-content">
						<span class="bigger-120">
							<span class="blue bolder">PainTrax</span>
							&copy; 2017
						</span>

						
					</div>

					<!-- /section:basics/footer -->
				</div>
			</div>
           
			<a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
				<i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
			</a>
         
   </div>
</body>
</html>

